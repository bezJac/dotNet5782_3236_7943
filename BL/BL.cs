using BL.BO;
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace BL
{
    public partial class BL : IBL.IBL
    {

        IDAL.IDal myDal;
        public static double DroneElecUseEmpty;
        public static double DroneElecUseLight;
        public static double DroneElecUseMedium;
        public static double DroneElecUseHeavy;
        public static double DroneHourlyChargeRate;
        public List<DroneInList> Drones;
        public BL()

        {
            Drones = new List<DroneInList>();
            myDal = new DalObject.DalObject();
            double[] temp = myDal.GetElectricUse().ToArray();
            DroneElecUseEmpty = temp[0];
            DroneElecUseLight = temp[1];
            DroneElecUseMedium = temp[2];
            DroneElecUseHeavy = temp[3];
            DroneHourlyChargeRate = temp[4];
            Random rnd = new Random();
            foreach (IDAL.DO.Drone dr in myDal.GetAllDrones())
            {
                if (myDal.GetAllParcels(p => p.DroneId != 0).ToList().Any(prc => prc.DroneId == dr.Id))
                {
                    IDAL.DO.Parcel parcel = myDal.GetAllParcels().ToList().Find(prc => prc.DroneId == dr.Id);
                    IDAL.DO.Customer sender = myDal.GetCustomer(parcel.SenderId);
                    IDAL.DO.Customer target = myDal.GetCustomer(parcel.TargetId);
                    Location senderLocation = createLocation(sender.Longitude, sender.Lattitude);
                    Location targetLocation = createLocation(target.Longitude, target.Lattitude);
                    IDAL.DO.BaseStation charge = getNearestBasestation(targetLocation);
                    Location station = createLocation(charge.Longitude, charge.Lattitude);
                    if (parcel.PickedUp == DateTime.MinValue)
                    {
                        IDAL.DO.BaseStation st = getNearestBasestation(senderLocation);
                        Location current = createLocation(st.Longitude, st.Lattitude);
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Delivery,
                            Battery = rnd.Next(getMinimalCharge(current, senderLocation, targetLocation, station, (WeightCategories)parcel.Weight), 101),
                            ParcelId = parcel.Id,
                            DroneLocation = current,
                        });
                    }
                    else
                    {
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Delivery,
                            Battery = rnd.Next(getMinimalCharge(senderLocation, senderLocation, targetLocation, station, (WeightCategories)parcel.Weight), 101),
                            ParcelId = parcel.Id,
                            DroneLocation = senderLocation,
                        });
                    }
                }
                else
                {
                    DroneStatus tmpStatus = (DroneStatus)rnd.Next(1, 3);
                    if (tmpStatus == DroneStatus.Available)
                    {
                        List<IDAL.DO.Parcel> deliveredParcels = myDal.GetAllParcels(prc => prc.Delivered != DateTime.MinValue).ToList();
                        Location current = GetCustomer(deliveredParcels.ElementAt(rnd.Next(0, deliveredParcels.Count())).TargetId).CustomerLocation;
                        IDAL.DO.BaseStation st = getNearestBasestation(current);
                        int tempBattery = (int)(Distance.GetDistance(current, createLocation(st.Longitude, st.Lattitude)) * DroneElecUseEmpty);
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Available,
                            Battery = rnd.Next(tempBattery, 101),
                            ParcelId = 0,
                            DroneLocation = current,
                        });
                    }
                    else
                    {
                        IEnumerable<IDAL.DO.BaseStation> tempSt = myDal.GetAllBaseStations();
                        int index = (int)rnd.Next(0, tempSt.Count());
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Maintenance,
                            Battery = rnd.Next(21),
                            ParcelId = 0,
                            DroneLocation = createLocation(tempSt.ElementAt(index).Longitude, tempSt.ElementAt(index).Lattitude),
                        });
                        IDAL.DO.BaseStation st = tempSt.ElementAt(index);
                        st.NumOfSlots--;
                        myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = dr.Id, StationId = st.Id });
                        myDal.UpdateBaseStation(st);

                    }
                }
            }
        }

        #region private methods for local  calculations
        private IDAL.DO.BaseStation getNearestBasestation(Location l)
        {
            double min = double.PositiveInfinity;
            double temp;
            int id = 0;
            foreach (IDAL.DO.BaseStation st in myDal.GetAllBaseStations(s => s.NumOfSlots > 0))
            {
                temp = Distance.GetDistance(createLocation(st.Longitude, st.Lattitude), l);
                if (temp < min)
                {
                    min = temp;
                    id = st.Id;
                }
            }
            return myDal.GetBaseStation(id);
        }
        private IDAL.DO.Parcel getNearestParcel(Location l, IEnumerable<IDAL.DO.Parcel> parcels)
        {
            double min = double.PositiveInfinity;
            double temp;
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
            foreach (IDAL.DO.Parcel prc in parcels)
            {
                IDAL.DO.Customer cs = myDal.GetCustomer(prc.SenderId);
                temp = Distance.GetDistance(createLocation(cs.Longitude, cs.Lattitude), l);
                if (temp < min)
                {
                    min = temp;
                    parcel = prc;
                }
            }
            return parcel;
        }
        private ParcelStatus getParcelStatus(IDAL.DO.Parcel pr)
        {
            if (pr.Scheduled == DateTime.MinValue)
                return ParcelStatus.Orderd;
            if (pr.PickedUp == DateTime.MinValue)
                return ParcelStatus.Linked;
            if (pr.Delivered == DateTime.MinValue)
                return ParcelStatus.PickedUp;
            return ParcelStatus.Delivered;
        }
        private bool checkDroneDistanceCoverage(DroneInList dr, Location sender, Location target, WeightCategories w)
        {
            IDAL.DO.BaseStation tmp = getNearestBasestation(target);
            double DroneToSender = Distance.GetDistance(dr.DroneLocation, sender) * DroneElecUseEmpty;
            double SenderToTarget = Distance.GetDistance(sender, target);
            double TargetToBaseStation = Distance.GetDistance(target, createLocation(tmp.Longitude, tmp.Lattitude)) * DroneElecUseEmpty;
            switch (w)
            {
                case WeightCategories.Light:
                    {
                        if (dr.Battery - DroneToSender - DroneElecUseLight * SenderToTarget - TargetToBaseStation > 0)
                            return true;
                        else
                            return false;
                    }
                case WeightCategories.Medium:
                    {
                        if (dr.Battery - DroneToSender - DroneElecUseMedium * SenderToTarget - TargetToBaseStation > 0)
                            return true;
                        else
                            return false;
                    }

                case WeightCategories.Heavy:
                    {
                        if (dr.Battery - DroneToSender - DroneElecUseHeavy * SenderToTarget - TargetToBaseStation > 0)
                            return true;
                        else
                            return false;
                    }
                default:
                    return false;


            }
        }
        private double getElectricUseForDrone(WeightCategories w)
        {
            switch (w)
            {
                case WeightCategories.Light:
                    return DroneElecUseLight;
                case WeightCategories.Medium:
                    return DroneElecUseMedium;
                case WeightCategories.Heavy:
                    return DroneElecUseHeavy;
                default:
                    return 0;
            }

        }
        private int getMinimalCharge(Location current, Location sender, Location target, Location station, WeightCategories w)
        {
            double currToSender = Distance.GetDistance(current, sender) * DroneElecUseEmpty;
            double SenderToTarget = Distance.GetDistance(sender, target);
            double TargetToSt = Distance.GetDistance(target, station) * DroneElecUseEmpty;
            switch (w)
            {
                case WeightCategories.Light:
                    {
                        SenderToTarget *= DroneElecUseLight;
                        break;
                    }
                case WeightCategories.Medium:
                    {
                        SenderToTarget *= DroneElecUseMedium;
                        break;
                    }
                case WeightCategories.Heavy:
                    {
                        SenderToTarget *= DroneElecUseHeavy;
                        break;
                    }
                default:
                    break;
            }

            return (int)(currToSender + SenderToTarget + TargetToSt);


        }
        private Location createLocation(double lon, double lat)
        {
            return new Location { Longtitude = lon, Lattitude = lat };
        }
        private BaseStation convertToBaseStation(IDAL.DO.BaseStation st)
        {
            return new BaseStation
            {
                Id = st.Id,
                Name = st.Name,
                StationLocation = createLocation(st.Longitude, st.Lattitude),
                NumOfSlots = st.NumOfSlots,
                DronesCharging = GetAllDronesCharging(st.Id),
            };
        }
        private Drone convertToDrone(DroneInList drone)
        {
            if (drone.ParcelId != 0)
            {
                return new Drone
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Status = drone.Status,
                    Battery = drone.Battery,
                    Parcel = GetParcelInDelivery(drone.ParcelId),
                    Location = drone.DroneLocation,
                };
            }
            else
            {
                return new Drone
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Status = drone.Status,
                    Battery = drone.Battery,
                    Parcel = null,
                    Location = drone.DroneLocation,
                };
            }
        }
        private Customer convertToCustomer(IDAL.DO.Customer cstmr)
        {
            return new Customer
            {
                Id = cstmr.Id,
                Name = cstmr.Name,
                Phone = cstmr.Phone,
                CustomerLocation = createLocation(cstmr.Longitude, cstmr.Lattitude),
                From = GetAllOutGoingDeliveries(cstmr.Id),
                To = GetAllIncomingDeliveries(cstmr.Id),
            };
        }
        private Parcel convertToParcel(IDAL.DO.Parcel prc)
        {
            if (prc.DroneId != 0)
            {
                return new Parcel
                {
                    Id = prc.Id,
                    Sender = GetCustomerInParcel(prc.SenderId),
                    Target = GetCustomerInParcel(prc.TargetId),
                    Weight = (WeightCategories)prc.Weight,
                    Priority = (Priority)prc.Priority,
                    Drone = GetDroneInParcel(prc.DroneId),
                    Ordered = prc.Requested,
                    Linked = prc.Scheduled,
                    PickedUp = prc.PickedUp,
                    Delivered = prc.Delivered,
                };
            }
            else
            {
                return new Parcel
                {
                    Id = prc.Id,
                    Sender = GetCustomerInParcel(prc.SenderId),
                    Target = GetCustomerInParcel(prc.TargetId),
                    Weight = (WeightCategories)prc.Weight,
                    Priority = (Priority)prc.Priority,
                    Drone = null,
                    Ordered = prc.Requested,
                    Linked = prc.Scheduled,
                    PickedUp = prc.PickedUp,
                    Delivered = prc.Delivered,
                };
            }
        }
        private ParcelInList convertToParcelInList(IDAL.DO.Parcel prc)
        {
            return new ParcelInList
            {
                Id = prc.Id,
                SenderName = myDal.GetCustomer(prc.SenderId).Name,
                TargetName = myDal.GetCustomer(prc.TargetId).Name,
                Weight = (WeightCategories)prc.Weight,
                Priority = (Priority)prc.Priority,
                Status = getParcelStatus(prc),
            };
        }
        #endregion





    }
}


