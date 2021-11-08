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
    public class BL : IBL.IBL
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
            myDal = new DalObject.DalObject();
            double[] temp = myDal.GetElectricUse().ToArray();
            DroneElecUseEmpty = temp[0];
            DroneElecUseLight = temp[1];
            DroneElecUseMedium = temp[2];
            DroneElecUseHeavy = temp[3];
            DroneHourlyChargeRate = temp[4];
            foreach (IDAL.DO.Drone dr in myDal.GetAllDrones())
            {


            }

        }

        public void AddBaseStation(BaseStation station)
        {
            if (myDal.GetAllBaseStations().Any(st => st.Id == station.Id))
                throw new BaseStationException($" {station.Id} exist already");
            myDal.AddBaseStation(new IDAL.DO.BaseStation
            {
                Id = station.Id,
                Name = station.Name,
                Longitude = station.StationLocation.Longtitude,
                Lattitude = station.StationLocation.Lattitude,
                NumOfSlots = station.NumOfSlots,


            });
        }
        public void AddDrone(Drone drone, int stationId)
        {
            if (myDal.GetAllDrones().Any(dr => dr.Id == drone.Id))
                throw new DroneException($" {drone.Id} exist already");
            if (!myDal.GetAllBaseStations().Any(st => st.Id == stationId))
                throw new BaseStationException($"base station - {stationId} dosen't exist");
            if (myDal.GetBaseStation(stationId).NumOfSlots == 0)
                throw new BaseStationException($"base station - {stationId} has no charging slots available");


            Random rnd = new Random();

            Drones.Add(new DroneInList
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Status = DroneStatus.Maintenance,
                Battery = rnd.Next(20, 41),
                DroneLocation = GetBasestationLocation(stationId),
            });
            myDal.AddDrone(new IDAL.DO.Drone
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,

            });
        }
        public void AddParcel(Parcel parcel)
        {

            if (!myDal.GetAllCustomers().Any(cs => cs.Id == parcel.Sender.Id))
                throw new CustomerException($"customer - {parcel.Sender.Id} dosen't exist");
            if (!myDal.GetAllCustomers().Any(cs => cs.Id == parcel.Target.Id))
                throw new CustomerException($"customer - {parcel.Target.Id} dosen't exist");

            parcel.Ordered = DateTime.Now;
            parcel.PickedUp = DateTime.MinValue;
            parcel.Linked = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            myDal.AddParcel(new IDAL.DO.Parcel
            {
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                DroneId = 0,
                Requested = parcel.Ordered,
                Scheduled = parcel.Linked,
                PickedUp = parcel.PickedUp,
                Delivered = parcel.Delivered,

            });
        }
        public void AddCustomer(Customer customer)
        {
            if (myDal.GetAllCustomers().Any(cst => cst.Id == customer.Id))
                throw new CustomerException($" {customer.Id} exist already");
            myDal.AddCustomer(new IDAL.DO.Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.CustomerLocation.Longtitude,
                Lattitude = customer.CustomerLocation.Lattitude,

            });
        }

        public void UpdateDrone(int id, string model)
        {
            if (!myDal.GetAllDrones().Any(dr => dr.Id == id))
                throw new DroneException($"drone- {id} doesn't exist");
            int index = Drones.FindIndex(dr => dr.Id == id);
            Drones[index].Model = model;
            myDal.UpdateDrone(new IDAL.DO.Drone { Id = id, Model = model, MaxWeight = (IDAL.DO.WeightCategories)Drones[index].MaxWeight });

        }
        public void UpdateBaseStation(int id, int count, string name)
        {
            if (myDal.GetAllBaseStations().Any(st => st.Id == id))
                throw new BaseStationException($"base station: {id} doesn't exist");
            if (GetBaseStation(id).DronesCharging.Count() > count)
                throw new BaseStationException($"base station: {id} Occupied slots exceed requested update");
            BaseStation station = GetBaseStation(id);

            myDal.UpdateBaseStation(new IDAL.DO.BaseStation
            {
                Id = id,
                Name = name,
                Longitude = station.StationLocation.Longtitude,
                Lattitude = station.StationLocation.Lattitude,
                NumOfSlots = station.NumOfSlots - station.DronesCharging.Count()
            });
        }
        public void UpdateCustomer(int id, string phone, string name)
        {
            if (!myDal.GetAllCustomers().Any(c => c.Id == id))
                throw new CustomerException($"cuatomer - {id} dosen't exist ");
            Customer cstmr = GetCustomer(id);
            myDal.UpdateCustomer(new IDAL.DO.Customer
            {
                Id = id,
                Name = name,
                Phone = phone,
                Longitude = cstmr.CustomerLocation.Longtitude,
                Lattitude = cstmr.CustomerLocation.Lattitude,

            });
        }
        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            List<BaseStation> temp = null;
            foreach (IDAL.DO.BaseStation station in myDal.GetAllBaseStations())
            {
                temp.Add(GetBaseStation(station.Id));
            }

            return temp;

        }
        public IEnumerable<Drone> GetAllDrones()
        {
            List<Drone> temp = null;
            foreach (DroneInList dr in Drones)
            {
                temp.Add(GetDrone(dr.Id));
            }
            return temp;
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> temp = null;
            foreach (IDAL.DO.Customer customer in myDal.GetAllCustomers())
            {
                temp.Add(GetCustomer(customer.Id));
            }
            return temp;
        }
        public IEnumerable<Parcel> GetAllParcels()
        {
            List<Parcel> temp = null;
            foreach (IDAL.DO.Parcel parcel in myDal.GetAllParcels())
            {
                temp.Add(GetParcel(parcel.Id));
            }
            return temp;
        }
        public IEnumerable<DroneCharge> GetAllDroneCharges(int stationId)
        {
            return myDal.GetAllDronecharges().Where(st => st.StationId == stationId).Select(Charge => new DroneCharge { Id = Charge.DroneId, Battery = GetDrone(Charge.DroneId).Battery });
        }
        public IEnumerable<ParcelAtCustomer> GetAllOutGoingDeliveries(int senderId)
        {
            var deliveris = myDal.GetAllParcels()
                .Where(p => p.SenderId == senderId)
                .Select(parcel =>
                    new ParcelAtCustomer
                    {
                        Id = parcel.Id,
                        Weight = (WeightCategories)parcel.Weight,
                        Priority = (Priority)parcel.Priority,
                        Status = (ParcelStatus)GetParcelStatusIndicator(parcel.Id),
                        CounterCustomer = GetCustomerInParcel(parcel.TargetId),

                    });
            return deliveris;
        }
        public IEnumerable<ParcelAtCustomer> GetAllIncomingDeliveries(int targetId)
        {
            var deliveris = myDal.GetAllParcels()
                .Where(p => p.TargetId == targetId)
                .Select(parcel =>
                    new ParcelAtCustomer
                    {
                        Id = parcel.Id,
                        Weight = (WeightCategories)parcel.Weight,
                        Priority = (Priority)parcel.Priority,
                        Status = (ParcelStatus)GetParcelStatusIndicator(parcel.Id),
                        CounterCustomer = GetCustomerInParcel(parcel.SenderId),

                    });
            return deliveris;
        }
        public IEnumerable<BaseStation> GetAllAvailableBaseStations()
        {
            List<BaseStation> temp = null;
            foreach (IDAL.DO.BaseStation station in myDal.GetAvailableCharge())
            {
                temp.Add(GetBaseStation(station.Id));
            }
            return temp;
        }
        public IEnumerable<Parcel> GetUnlinkedParcel()
        {
            List<Parcel> temp = null;
            foreach (IDAL.DO.Parcel parcel in myDal.GetUnlinkedParcels())
            {
                temp.Add(GetParcel(parcel.Id));
            }
            return temp;
        }
        public IEnumerable<Parcel> GetAllUnlinkedParcels()
        {
            List<Parcel> temp = null;
            foreach (IDAL.DO.Parcel prc in myDal.GetUnlinkedParcels())
            {
                temp.Add(GetParcel(prc.Id));
            }
            return temp;
        }

        public BaseStation GetBaseStation(int id)
        {
            if (!myDal.GetAllBaseStations().Any(b => b.Id == id))
                throw new BaseStationException($"base station -  {id} dosen't exist ");
            BaseStation temp = null;
            foreach (IDAL.DO.BaseStation st in myDal.GetAllBaseStations())
            {
                if (st.Id == id)
                {
                    Location l = new Location(); ;
                    l.Longtitude = st.Longitude;
                    l.Lattitude = st.Lattitude;
                    temp = new BaseStation
                    {
                        Id = st.Id,
                        Name = st.Name,
                        StationLocation = l,
                        NumOfSlots = st.NumOfSlots,
                        DronesCharging = GetAllDroneCharges(id),
                    };
                }
            }
            if (temp == null)
                throw new BaseStationException($"base station - {id} wasen't found");
            return temp;
        }
        public Drone GetDrone(int id)
        {

            if (!Drones.Any(dr => dr.Id == id))
            {
                throw new DroneException("id not found");
            }
            Drone temp = null;
            foreach (DroneInList dr in Drones)
            {
                if (dr.Id == id)
                {
                    if (GetAllParcels().Any(prc => prc.Drone.Id == dr.Id))
                    {
                        temp = new Drone
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = dr.MaxWeight,
                            Status = dr.Status,
                            Battery = dr.Battery,
                            Parcel = GetParcelInDelivery(dr.ParcelId),
                            Location = dr.DroneLocation,

                        };
                    }
                    else
                    {
                        temp = new Drone
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = dr.MaxWeight,
                            Status = dr.Status,
                            Battery = dr.Battery,
                            Parcel = null,
                            Location = dr.DroneLocation,
                        };
                    }
                }
             
                break;
            }
            return temp;
        }
        public Customer GetCustomer(int id)
        {
            if (!myDal.GetAllCustomers().Any(c => c.Id == id))
                throw new CustomerException($"cuatomer -  {id} dosen't exist ");
            Customer temp = null;
            foreach (IDAL.DO.Customer customer in myDal.GetAllCustomers())
            {
                if (customer.Id == id)
                {
                    Location l = new Location(); ;
                    l.Longtitude = customer.Longitude;
                    l.Lattitude = customer.Lattitude;
                    temp = new Customer
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Phone = customer.Phone,
                        CustomerLocation = l,
                        From = GetAllOutGoingDeliveries(customer.Id),
                        To = GetAllIncomingDeliveries(customer.Id),

                    };
                }
            }
            return temp;

        }
        public Parcel GetParcel(int id)
        {
            if (!myDal.GetAllParcels().Any(p => p.Id == id))
                throw new CustomerException($"parcel -  {id} dosen't exist ");
            var parcel = new Parcel() ;
            if (GetParcelInList(id).Status == ParcelStatus.Linked || GetParcelInList(id).Status == ParcelStatus.PickedUp)
            {
                 parcel = (Parcel)myDal.GetAllParcels()
                    .Where(p => p.Id == id)
                    .Select(prc =>
                        new Parcel
                        {
                            Id = prc.Id,
                            Sender = GetCustomer(prc.SenderId),
                            Target = GetCustomer(prc.TargetId),
                            Weight = (WeightCategories)prc.Weight,
                            Priority = (Priority)prc.Priority,
                            Drone = GetDroneInParcel(prc.DroneId),
                            Ordered = prc.Requested,
                            Linked = prc.Scheduled,
                            PickedUp = prc.PickedUp,
                            Delivered = prc.Delivered,
                        });
            }
            else
            {
                parcel = (Parcel)myDal.GetAllParcels()
                    .Where(p => p.Id == id)
                    .Select(prc =>
                        new Parcel
                        {
                            Id = prc.Id,
                            Sender = GetCustomer(prc.SenderId),
                            Target = GetCustomer(prc.TargetId),
                            Weight = (WeightCategories)prc.Weight,
                            Priority = (Priority)prc.Priority,
                            Drone = null,
                            Ordered = prc.Requested,
                            Linked = prc.Scheduled,
                            PickedUp = prc.PickedUp,
                            Delivered = prc.Delivered,
                        });
            }
            return parcel;
        }
        public CustomerInParcel GetCustomerInParcel(int id)
        {
            var customer = myDal.GetAllCustomers()
                .Where(c => c.Id == id)
                .Select(cstmr =>
                    new CustomerInParcel
                    {
                        Id = myDal.GetCustomer(id).Id,
                        Name = myDal.GetCustomer(id).Name,
                    });
            return (CustomerInParcel)customer;
        }
        public ParcelInDelivery GetParcelInDelivery(int id)
        {
            ParcelInList prc1 = GetParcelInList(id);
            Parcel prc2 = GetParcel(id);
            bool flag=false;
            if (prc1.Status == ParcelStatus.PickedUp)
                flag = true;

            return new ParcelInDelivery
                {
                Id=prc1.Id,
                Status=flag,
                Priority=prc1.Priority,
                Weight=prc1.Weight,
                Sender=GetCustomerInParcel(prc2.Sender.Id),
                Target= GetCustomerInParcel(prc2.Target.Id),
                SenderLocation=prc2.Sender.CustomerLocation,
                TargetLocation=prc2.Target.CustomerLocation,
                DeliveryDistance=Distance.GetDistance(prc2.Sender.CustomerLocation, prc2.Target.CustomerLocation),

            };
        }
        public ParcelInList GetParcelInList(int id)
        {
            if (!myDal.GetAllParcels().Any(p => p.Id == id))
                throw new ParcelException($"parcel -  {id} dosen't exist ");
            var parcel = myDal.GetAllParcels()
                .Where(p => p.Id == id)
                .Select(prc =>
                    new ParcelInList
                    {
                        Id = prc.Id,
                        SenderName = GetCustomer(prc.SenderId).Name,
                        TargetName = GetCustomer(prc.TargetId).Name,
                        Weight = (WeightCategories)prc.Weight,
                        Priority = (Priority)prc.Priority,
                        Status=(ParcelStatus)GetParcelStatusIndicator(prc.Id),
                    });
            return (ParcelInList)parcel;
        }
        public DroneInParcel GetDroneInParcel(int id)
        {
            var drone = myDal.GetAllParcels()
                .Where(d => d.DroneId == id)
                .Select(dr => new DroneInParcel
                {
                    Id = GetDrone(id).Id,
                    Battery = GetDrone(id).Battery,
                    DroneLocation = GetDrone(id).Location,
                });
            return (DroneInParcel)drone;

        }
        public Location GetBasestationLocation(int id)
        {
            return GetBaseStation(id).StationLocation;
        }





        /// <summary>
        /// send a drone to charge at nearest base station
        /// </summary>
        /// <param name="id"> id of drone </param>
        /// <exception cref = "DroneException"> </exception>
        public void ChargeDrone(int id)
        {
            // check if drone exists
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($" {id} dosen't exist already");

            int index = Drones.FindIndex(x => (x.Id == id));

            // check if drone is available to charge
            if (Drones[index].Status == DroneStatus.Maintenance)
                throw new DroneException($"drone - {id} is already charging");
            if (Drones[index].Status == DroneStatus.Delivery)
                throw new DroneException($"drone - {id} is en route , cannot be charged right now");

            // find nearest base station
            int stationId = GetNearestBasestationID(Drones[index].DroneLocation , GetAllAvailableBaseStations());
            BaseStation st = GetBaseStation(stationId);

            double distance = Distance.GetDistance( st.StationLocation, Drones[index].DroneLocation);

            // check if drone has enough battery to cover the  distance
            if ((Drones[index].Battery - distance* DroneElecUseEmpty) <= 0)
                throw new DroneException($"drone - {id} does not have enough battery to reach nearest base station");
            
            // update drone's details 
            Drones[index].Battery -= (int)(distance * DroneElecUseEmpty);
            Drones[index].DroneLocation.Longtitude = st.StationLocation.Longtitude;
            Drones[index].DroneLocation.Lattitude = st.StationLocation.Lattitude;
            Drones[index].Status = DroneStatus.Maintenance;

            // update necessary details in datasource
            myDal.ChargeDrone(myDal.GetBaseStation(st.Id), myDal.GetDrone(id));   
        }
        public void DischargeDrone(int id, int time)
        {
            // check if drone exists
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($" {id} dosen't exist ");
            int index = Drones.FindIndex(x => (x.Id == id));
            if ((Drones[index].Status == DroneStatus.Available))
                throw new DroneException($"drone - {id} is currently  not at  charging dock");
            if (Drones[index].Status == DroneStatus.Delivery)
                throw new DroneException($"drone - {id} is en route , currently  not at  charging dock");

            Drones[index].Battery = Math.Max(Drones[index].Battery + (int)DroneHourlyChargeRate * time, 100);
            Drones[index].Status = DroneStatus.Available;

            int stationId = myDal.GetDroneCharge(id).StationId;
            myDal.ReleaseDroneCharge(myDal.GetBaseStation(stationId), myDal.GetDrone(id));
        }
        public void LinkDroneToParcel(int id)
        {
            if (Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            Drone dr = GetDrone(id);
            int index = GetAllDrones().ToList().FindIndex(dr => dr.Id == id);
            List<Parcel> unlinked = GetUnlinkedParcel().ToList();
            if (unlinked.Count==0)
                throw new ParcelException($"no available parcels to link");
            for (int i = (int)Priority.Emergency; i >0; i--)
            {
                List<Parcel> temp = unlinked.FindAll(prc => prc.Priority == (Priority)i);
                if (temp.Count > 0)
                {
                    for (int j = (int)dr.MaxWeight; j > 0; j--)
                    {
                        temp = temp.FindAll(prc => prc.Weight == (WeightCategories)j);
                        if (temp.Count() > 0)
                        {
                            Parcel prc = GetParcel(GetNearestParcelID(dr.Location, temp));
                            if (CheckDroneDistanceCoverage(dr, prc, dr.MaxWeight))
                            {
                                myDal.LinkParcelToDrone(myDal.GetParcel(prc.Id), myDal.GetDrone(dr.Id));
                                Drones[index].Status = DroneStatus.Delivery;
                                Drones[index].ParcelId = prc.Id;
                                return;
                            }

                        }
                    }
                }
            }
             throw new DroneException($"drone - {id} has no compatible parcel to link");
        }
        public void DroneParcelPickUp(int id)
        {
            if (Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            Drone dr = GetDrone(id);
            if (dr.Parcel==null)
                throw new DroneException($"Drone - {id} isen't linked yet to a parcel");
            if((dr.Parcel.Status==true))
                throw new DroneException($"Drone - {id} already picked up parcel");
         
            int index = GetAllDrones().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.SenderLocation) * DroneElecUseEmpty);
            Drones[index].DroneLocation = dr.Parcel.SenderLocation;
            myDal.DroneParcelPickup(myDal.GetParcel(dr.Parcel.Id));
        }
        public void DroneParcelDelivery(int id)
        {
            if (Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            Drone dr = GetDrone(id);
            if (dr.Parcel == null)
                throw new DroneException($"Drone - {id} isen't linked yet to a parcel");
            Parcel prc = GetParcel(dr.Parcel.Id);
            if(prc.Delivered != DateTime.MinValue)
                throw new DroneException($"Drone - {id} already delievered  parcel");

            int index = GetAllDrones().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.TargetLocation) * GetElectricUseForDrone(prc.Weight));
            Drones[index].DroneLocation = dr.Parcel.TargetLocation;
            Drones[index].Status = DroneStatus.Available;

            myDal.ParcelDelivery(myDal.GetParcel(dr.Parcel.Id));
        }
        // extra functions
        /// <summary>
        /// Get id of nearest base station with available charging slots from 
        /// a specific location
        /// </summary>
        /// <param name="l"> location to calculate from </param>
        /// <param name="stations"> list of all base stations</param>
        /// <returns> int </returns>
        public int GetNearestBasestationID(Location l ,IEnumerable<BaseStation> stations)
        {
            
            double min = double.PositiveInfinity;
            double temp;
            int id = stations.First().Id;
            foreach (BaseStation st in stations)
            {
                temp = Distance.GetDistance(st.StationLocation, l);
                if (temp < min && st.NumOfSlots > 0)
                {
                    min = temp;
                    id = st.Id;
                }
            }
            return id;
        }
        public int GetNearestParcelID(Location l, IEnumerable<Parcel> parcels)
        {
             
            double min = double.PositiveInfinity;
            double temp;
            int id = parcels.First().Id;
            foreach (Parcel prc in parcels)
            {
                temp = Distance.GetDistance(prc.Sender.CustomerLocation, l);
                if (temp < min )
                {
                    min = temp;
                    id = prc.Id;
                }
            }
            return id;
        }
        public int GetParcelStatusIndicator(int id)
        {
            if (myDal.GetParcel(id).Scheduled == DateTime.MinValue)
                return 1;
            if (myDal.GetParcel(id).PickedUp == DateTime.MinValue)
                return 2;
            if (myDal.GetParcel(id).Delivered == DateTime.MinValue)
                return 3;
            return 4;
        }
        public bool CheckDroneDistanceCoverage(Drone dr, Parcel prc, WeightCategories w)
        {
            double DroneToSender = Distance.GetDistance(dr.Location, prc.Sender.CustomerLocation)*DroneElecUseEmpty;
            double SenderToTarget = Distance.GetDistance(prc.Sender.CustomerLocation, prc.Target.CustomerLocation);
            double TargetToBaseStation = Distance.GetDistance(prc.Target.CustomerLocation, GetBasestationLocation(GetNearestBasestationID(prc.Target.CustomerLocation,GetAllAvailableBaseStations())))*DroneElecUseEmpty;
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
        public double GetElectricUseForDrone(WeightCategories w)
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

    }
    
}
