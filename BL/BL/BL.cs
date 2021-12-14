﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using BlApi;
using BO;

namespace BL
{
    /// <summary>
    /// class manages BL logic for DAL.
    /// </summary>
    public partial class BL : IBL
    {
        private static  BL instance;
        private static object locker = new object();

        public static BL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                            instance = new BL();
                    }
                }
                return instance;
            }
        }
        private DalApi.IDal myDal;
        private List<DroneInList> drones;
        private static double droneElecUseEmpty;
        private static double droneElecUseLight;
        private static double droneElecUseMedium;
        private static double droneElecUseHeavy;
        private static double droneHourlyChargeRate;

        
        /// <summary>
        /// cunstroctor
        /// </summary>
        BL()

        {
            drones = new List<DroneInList>();
            myDal = DalApi.DalFactory.GetDal();
            double[] temp = myDal.GetElectricUse().ToArray();
            droneElecUseEmpty = temp[0];
            droneElecUseLight = temp[1];
            droneElecUseMedium = temp[2];
            droneElecUseHeavy = temp[3];
            droneHourlyChargeRate = temp[4];
            Random rnd = new();
            foreach (DO.Drone dr in myDal.GetAllDrones())
            { 
                // drone is linked to a parcel
                if (myDal.GetAllParcels(p => p.DroneId != 0).Any(prc => prc.DroneId == dr.Id))
                {
                    DO.Parcel parcel = myDal.GetAllParcels(p => p.DroneId != 0).FirstOrDefault(prc => prc.DroneId == dr.Id);
                    DO.Customer sender = myDal.GetCustomer(parcel.SenderId);
                    DO.Customer target = myDal.GetCustomer(parcel.TargetId);
                    Location senderLocation = createLocation(sender.Longitude, sender.Lattitude);
                    Location targetLocation = createLocation(target.Longitude, target.Lattitude);
                    DO.BaseStation charge = getNearestAvailableBasestation(targetLocation);
                    Location chargingStation = createLocation(charge.Longitude, charge.Lattitude);

                    // drone did not pick up parcel yet - set location to nearest base station to sender
                    if (parcel.PickedUp == null)
                    {
                        DO.BaseStation st = getNearestAvailableBasestation(senderLocation);
                        Location current = createLocation(st.Longitude, st.Lattitude);
                        drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Delivery,
                            Battery = rnd.Next(getMinimalCharge(current, senderLocation, targetLocation, chargingStation, (WeightCategories)parcel.Weight), 101),
                            ParcelId = parcel.Id,
                            DroneLocation = current,
                        });
                    }
                    // drone already picked up parcel - set location at sender
                    else
                    {
                        drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Delivery,
                            Battery = rnd.Next(getMinimalCharge(senderLocation, senderLocation, targetLocation, chargingStation, (WeightCategories)parcel.Weight), 101),
                            ParcelId = parcel.Id,
                            DroneLocation = senderLocation,
                        });
                    }
                }

                // drone isn't linked to a parcel
                else
                {
                    // randomly set drone status to either available or maintanence
                    DroneStatus tmpStatus = (DroneStatus)rnd.Next(1, 3);

                    // if status is avilable set location to one of customers with delivered parcel
                    if (tmpStatus == DroneStatus.Available)
                    {
                        List<DO.Parcel> deliveredParcels = myDal.GetAllParcels(prc => prc.Delivered != null).ToList();
                        Location current = GetCustomer(deliveredParcels.ElementAt(rnd.Next(0, deliveredParcels.Count)).TargetId).CustomerLocation;
                        DO.BaseStation st = getNearestAvailableBasestation(current);
                        int tempBattery = (int)(Distance.GetDistance(current, createLocation(st.Longitude, st.Lattitude)) * droneElecUseEmpty);
                        drones.Add(new DroneInList
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

                    // status is maintanence - set drone location to randomly selected station
                    else
                    {
                        IEnumerable<DO.BaseStation> tempSt = myDal.GetAllBaseStations();
                        int index = (int)rnd.Next(0, tempSt.Count());
                        drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Maintenance,
                            Battery = rnd.Next(21),
                            ParcelId = 0,
                            DroneLocation = createLocation(tempSt.ElementAt(index).Longitude, tempSt.ElementAt(index).Lattitude),
                        });
                        // drone was set as charging - update DAL with chrge details
                        DO.BaseStation st = tempSt.ElementAt(index);
                        st.NumOfSlots--;
                        myDal.AddDroneCharge(new DO.DroneCharge { DroneId = dr.Id, StationId = st.Id ,EntranceTime = DateTime.Now.Subtract(new TimeSpan(rnd.Next(2),rnd.Next(30),0))});
                        myDal.UpdateBaseStation(st);

                    }
                }
            }
        }

        #region private methods for local  calculations
        /// <summary>
        /// calculate nearest base station to specific location 
        /// </summary>
        /// <param name="l"> location to calculate distance from </param>
        /// <returns> IDAL.DO.BaseStation instance of nearest station </returns>
        private DO.BaseStation getNearestAvailableBasestation(Location l)
        {
            double min = double.PositiveInfinity;
            double distance;
            DO.BaseStation tmpStation = new();
            foreach (DO.BaseStation st in myDal.GetAllBaseStations(s => s.NumOfSlots > 0))
            {
                distance = Distance.GetDistance(createLocation(st.Longitude, st.Lattitude), l);
                if (distance < min)
                {
                    tmpStation = st;
                    min = distance;
                }
            }
            return tmpStation;
        }

        /// <summary>
        /// calculate parcel status indicated by delivery stages time values
        /// </summary>
        /// <param name="pr"> parcel to calculate status of </param>
        /// <returns> ParcelStatus enum value </returns>
        private  ParcelStatus getParcelStatus(DO.Parcel pr)
        {
            if(pr.Scheduled == null)
                return ParcelStatus.Orderd;
            if (pr.PickedUp == null)
                return ParcelStatus.Linked;
            if (pr.Delivered == null)
                return ParcelStatus.PickedUp;
            return ParcelStatus.Delivered;
        }

        /// <summary>
        /// check if a drone has enough battery to execute a specific delivery of a parcel
        /// </summary>
        /// <param name="dr"> the drone</param>
        /// <param name="sender"> location of parcel's sender customer  </param>
        /// <param name="target"> location of parcel's target customer </param>
        /// <param name="w"> weight category of parcel </param>
        /// <returns> bool </returns>
        private  bool checkDroneDistanceCoverage(DroneInList dr, Location sender, Location target, WeightCategories w)
        {
            // get nearest station to target with availability to charge 
            DO.BaseStation tmp = getNearestAvailableBasestation(target);
            // claculate distance from drone current location to sender
            double DroneToSender = Distance.GetDistance(dr.DroneLocation, sender);
            // calculate distance from sender to target
            double SenderToTarget = Distance.GetDistance(sender, target);
            // calculate distance from target to base station for charge
            double TargetToBaseStation = Distance.GetDistance(target, createLocation(tmp.Longitude, tmp.Lattitude));
            Console.WriteLine(DroneToSender + SenderToTarget + TargetToBaseStation);
            switch (w)
            {
                case WeightCategories.Light:
                    {
                        if (dr.Battery - (DroneToSender * droneElecUseEmpty) - (droneElecUseLight * SenderToTarget) - (TargetToBaseStation * droneElecUseEmpty) > 0)
                            return true;
                        else
                            return false;
                    }
                case WeightCategories.Medium:
                    {
                        if (dr.Battery - (DroneToSender * droneElecUseEmpty) - (droneElecUseMedium * SenderToTarget) - (TargetToBaseStation * droneElecUseEmpty) > 0)
                            return true;
                        else
                            return false;
                    }

                case WeightCategories.Heavy:
                    {
                        if (dr.Battery - (DroneToSender * droneElecUseEmpty) - (droneElecUseHeavy * SenderToTarget) - (TargetToBaseStation * droneElecUseEmpty) > 0)
                            return true;
                        else
                            return false;
                    }
                default:
                    return false;


            }
        }

        /// <summary>
        /// calculate electric use per hour for a given drone
        /// </summary>
        /// <param name="w"> dron max weight category</param>
        /// <returns> double </returns>
        private  double getElectricUseForDrone(WeightCategories w)
        {
            return w switch
            {
                WeightCategories.Light => droneElecUseLight,
                WeightCategories.Medium => droneElecUseMedium,
                WeightCategories.Heavy => droneElecUseHeavy,
                _ => 0,
            };
        }

        /// <summary>
        /// calculate minimal battery charge value for a drone to execute a delivery
        /// </summary>
        /// <param name="current"> current location of drone </param>
        /// <param name="sender"> location of sender customer </param>
        /// <param name="target"> location of target customer </param>
        /// <param name="station"> location post delivery charging station </param>
        /// <param name="w"> parcel weight category </param>
        /// <returns>  int - minimal battery charge </returns>
        private int getMinimalCharge(Location current, Location sender, Location target, Location station, WeightCategories w)
        {
            double currToSender = Distance.GetDistance(current, sender) ;
            double SenderToTarget = Distance.GetDistance(sender, target);
            double TargetToSt = Distance.GetDistance(target, station);
            switch (w)
            {
                case WeightCategories.Light:
                    {
                        SenderToTarget *= droneElecUseLight;
                        break;
                    }
                case WeightCategories.Medium:
                    {
                        SenderToTarget *= droneElecUseMedium;
                        break;
                    }
                case WeightCategories.Heavy:
                    {
                        SenderToTarget *= droneElecUseHeavy;
                        break;
                    }
                default:
                    break;
            }

            return (int)((currToSender * droneElecUseEmpty ) + SenderToTarget + (TargetToSt* droneElecUseEmpty));


        }

        /// <summary>
        /// create a location class object from a longtitude and lattitude coordinates
        /// </summary>
        /// <param name="lon"> longtitiude coordinate </param>
        /// <param name="lat"> lattitude coordinate </param>
        /// <returns> location class containing both coordinates</returns>
        private Location createLocation(double lon, double lat)
        {
            return new Location { Longtitude = lon, Lattitude = lat };
        }

        /// <summary>
        /// convert DAL BaseStation struct to a BL BaseStation object
        /// </summary>
        /// <param name="st"> DAL BaseStation </param>
        /// <returns> BL BaseStation</returns>
        private BaseStation convertToBaseStation(DO.BaseStation st)
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

        /// <summary>
        /// convert BL BaseStation object to a BL BaseStationInList object
        /// </summary>
        /// <param name="st"> DAL BaseStation </param>
        /// <returns>  the base station in BL BaseStationInList representation</returns>
        private BaseStationInList convertToBaseStationInList(BaseStation st)
        {
            return new BaseStationInList
            {
                Id = st.Id,
                Name = st.Name,
                AvailableSlots = st.NumOfSlots,
                OccupiedSlots = st.DronesCharging.Count(),
            };
        }

        /// <summary>
        /// convert BL DroneInList object to a BL Drone object
        /// </summary>
        /// <param name="drone"> BL DroneInList </param>
        /// <returns> BL Drone </returns>
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

        /// <summary>
        /// convert DAL Customer struct to a BL Customer object
        /// </summary>
        /// <param name="cstmr"> DAL Customer </param>
        /// <returns> BL Customer </returns>
        private Customer convertToCustomer(DO.Customer cstmr)
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

        /// <summary>
        /// convert BL Customer object to a BL CustomerInList object
        /// </summary>
        /// <param name="cstmr"> DAL Customer </param>
        /// <returns> BL CustomerInList </returns>
        private CustomerInList convertToCustomerInList(Customer cstmr)
        {
            // calculate number of parcels from customer no delivered yet
            int fromCount = cstmr.From.Count(prc => prc.Status != ParcelStatus.Delivered);
            // calculate number of parcels to customer that already arrived
            int toCount = cstmr.To.Count(prc => prc.Status == ParcelStatus.Delivered);
            return new CustomerInList
            {
                Id = cstmr.Id,
                Name = cstmr.Name,
                Phone = cstmr.Phone,
                SentCount = fromCount,
                DeliveredCount = cstmr.From.Count() - fromCount,
                ExpectedCount = cstmr.To.Count() - toCount,
                RecievedCount = toCount,

            };
        }

        /// <summary>
        /// convert DAL Parcel struct to a BL Parcel object
        /// </summary>
        /// <param name="prc"> DAL Parcel </param>
        /// <returns> BL Parcel </returns>
        private Parcel convertToParcel(DO.Parcel prc)
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

        /// <summary>
        /// convert DAL Parcel struct to a BL ParcelInList object
        /// </summary>
        /// <param name="prc"> DAL Parcel </param>
        /// <returns> BL ParcelInList </returns>
        private ParcelInList convertToParcelInList(DO.Parcel prc )
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

