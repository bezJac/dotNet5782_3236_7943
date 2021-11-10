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
                    IEnumerable<IDAL.DO.Parcel> Parcel = myDal.GetAllParcels().Where(prc => prc.DroneId == dr.Id);
                    IDAL.DO.Parcel prc = Parcel.First();
                    Parcel tempParcel = new Parcel
                    {
                        Id = prc.Id,
                        Sender = GetCustomer(prc.SenderId),
                        Target = GetCustomer(prc.TargetId),
                        Weight = (WeightCategories)prc.Weight,
                        Priority = (Priority)prc.Priority,
                        Ordered = prc.Requested,
                        Linked = prc.Scheduled,
                        PickedUp = prc.PickedUp,
                        Delivered = prc.Delivered,

                    };
                    if (tempParcel.PickedUp == DateTime.MinValue)
                    {
                        Location tempLo = GetBaseStation(GetNearestBasestationID(tempParcel.Sender.CustomerLocation, GetAllBaseStations())).StationLocation;
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Delivery,
                            Battery = rnd.Next(GetMinimalCharge(tempLo, tempParcel), 101),
                            ParcelId = tempParcel.Id,
                            DroneLocation = tempLo,

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
                            Battery = rnd.Next(GetMinimalCharge(tempParcel.Sender.CustomerLocation, tempParcel), 101),
                            ParcelId = tempParcel.Id,
                            DroneLocation = tempParcel.Sender.CustomerLocation,

                        });

                    }


                }
                else
                {
                    DroneStatus tmpStatus = (DroneStatus)rnd.Next(1, 3);
                    if (tmpStatus == DroneStatus.Available)
                    {
                        List<Parcel> tempCustomers = GetAllParcels().ToList();
                        foreach (Parcel item in tempCustomers)
                        {
                            if (item.Delivered != DateTime.MinValue)
                                tempCustomers.Add(item);
                        }

                        Console.WriteLine(tempCustomers.Count());
                        Location tempLo = tempCustomers.ElementAt(rnd.Next(0, tempCustomers.Count() - 1)).Target.CustomerLocation;
                        int tempBattery = (int)(Distance.GetDistance(tempLo, GetBaseStation(GetNearestBasestationID(tempLo, GetAllAvailableBaseStations())).StationLocation) * DroneElecUseEmpty);
                        if (tempBattery > 99)
                            tempBattery = 75;
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Available,
                            Battery = rnd.Next(tempBattery, 101),
                            ParcelId = 0,
                            DroneLocation = tempLo,

                        });
                    }
                    else
                    {
                        IEnumerable<BaseStation> tempSt = GetAllAvailableBaseStations();
                        int index = (int)rnd.Next(0, tempSt.Count());
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Maintenance,
                            Battery = rnd.Next(20, 41),
                            ParcelId = 0,
                            DroneLocation = tempSt.ElementAt(index).StationLocation,

                        });
                        myDal.ChargeDrone(myDal.GetBaseStation(tempSt.ElementAt(index).Id), myDal.GetDrone(dr.Id));
                    }
                }

            }

        }

        #region Adding new entity methods
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
            myDal.ChargeDrone(myDal.GetBaseStation(stationId), myDal.GetDrone(drone.Id));
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
        public Location CreateLocation(double lon, double lat)
        {
            return new Location { Longtitude = lon, Lattitude = lat };
        }
        #endregion
        #region Updating a Entity methods 
        #region user specific entity static details update 
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
        #endregion
        #region actions causing updates
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
            int stationId = GetNearestBasestationID(Drones[index].DroneLocation, GetAllBaseStations().ToList().FindAll(st=>st.NumOfSlots>0));
            BaseStation st = GetBaseStation(stationId);

            double distance = Distance.GetDistance(st.StationLocation, Drones[index].DroneLocation);

            // check if drone has enough battery to cover the  distance
            if ((Drones[index].Battery - distance * DroneElecUseEmpty) <= 0)
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
            if (unlinked.Count == 0)
                throw new ParcelException($"no available parcels to link");
            for (int i = (int)Priority.Emergency; i > 0; i--)
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
            if (dr.Parcel == null)
                throw new DroneException($"Drone - {id} isen't linked yet to a parcel");
            if ((dr.Parcel.Status == true))
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
            if (prc.Delivered != DateTime.MinValue)
                throw new DroneException($"Drone - {id} already delievered  parcel");

            int index = GetAllDrones().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.TargetLocation) * GetElectricUseForDrone(prc.Weight));
            Drones[index].DroneLocation = dr.Parcel.TargetLocation;
            Drones[index].Status = DroneStatus.Available;

            myDal.ParcelDelivery(myDal.GetParcel(dr.Parcel.Id));
        }
        #endregion
        #endregion
        #region get information of Entity/Entities
        #region get list of elements
        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            List<BaseStation> temp = new List<BaseStation>();
            foreach (IDAL.DO.BaseStation station in myDal.GetAllBaseStations())
            {
                temp.Add(GetBaseStation(station.Id));
            }

            return temp;

        }
        public IEnumerable<BaseStationInList> GetALLBaseStationInList()
        {

            List<BaseStationInList> tmp = new List<BaseStationInList>();
            foreach (BaseStation station in GetAllBaseStations())
            {
                tmp.Add(new BaseStationInList
                {
                    Id = station.Id,
                    Name = station.Name,
                    AvailableSlots = station.NumOfSlots,
                    OccupiedSlots = station.DronesCharging.Count(),
                });
            }
            return tmp;
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
        public IEnumerable<DroneInList> GetAllDroneInLists()
        {
            return Drones;
        }
        public IEnumerable<CustomerInList> GetAllCustomerInList()
        {
            List<CustomerInList> tmp = new List<CustomerInList>();
            foreach (Customer cs in GetAllCustomers())
            {
                int sum1 = cs.From.Count(prc => prc.Status != ParcelStatus.Delivered);
                int sum2 = cs.To.Count(prc => prc.Status == ParcelStatus.Delivered);
                tmp.Add(new CustomerInList
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    Phone = cs.Phone,
                    SentCount = sum1,
                    DeliveredCount = cs.From.Count() - sum1,
                    RecievedCount = sum2,
                    ExpectedCount = cs.To.Count() - sum2,
                });
            }
            return tmp;
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> temp = new List<Customer>();
            foreach (IDAL.DO.Customer customer in myDal.GetAllCustomers())
            {
                temp.Add(GetCustomer(customer.Id));
            }
            return temp;
        }
        public IEnumerable<Parcel> GetAllParcels()
        {
            List<Parcel> temp = new List<Parcel>();
            foreach (IDAL.DO.Parcel parcel in myDal.GetAllParcels())
            {
                temp.Add(GetParcel(parcel.Id));
            }
            return temp;
        }
        public IEnumerable<DroneCharge> GetAllDroneCharges(int stationId)
        {
            return myDal.GetAllDronecharges(st => st.StationId == stationId).Select(Charge => new DroneCharge { Id = Charge.DroneId, Battery = GetDrone(Charge.DroneId).Battery });
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
                        Status = GetParcelStatus(parcel.Scheduled, parcel.PickedUp, parcel.Delivered),
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
                        Status = GetParcelStatus(parcel.Scheduled,parcel.PickedUp,parcel.Delivered),
                        CounterCustomer = GetCustomerInParcel(parcel.SenderId),

                    });
            return deliveris;
        }
        public IEnumerable<BaseStation> GetAllAvailableBaseStations()
        {
            List<BaseStation> temp = new List<BaseStation>();
            foreach (IDAL.DO.BaseStation station in myDal.GetAllBaseStations(st => st.NumOfSlots>0))
            {
                temp.Add(GetBaseStation(station.Id));
            }
            return temp;
        }
        public IEnumerable<Parcel> GetUnlinkedParcel()
        {
            List<Parcel> temp = new List<Parcel>();
            foreach (IDAL.DO.Parcel parcel in myDal.GetAllParcels(p => p.DroneId==0))
            {
                temp.Add(GetParcel(parcel.Id));
            }
            return temp;
        }
        public IEnumerable<Parcel> GetAllUnlinkedParcels()
        {
            List<Parcel> temp = new List<Parcel>();
            foreach (IDAL.DO.Parcel prc in myDal.GetAllParcels(p => p.DroneId != 0))
            {
                temp.Add(GetParcel(prc.Id));
            }
            return temp;
        }
        public IEnumerable<ParcelInList> GetAllParcelInList()
        {
            List<ParcelInList> tmp = new List<ParcelInList>();
            foreach (IDAL.DO.Parcel p in myDal.GetAllParcels())
            {
                tmp.Add(new ParcelInList
                {
                    Id = p.Id,
                    SenderName = myDal.GetCustomer(p.SenderId).Name,
                    TargetName = myDal.GetCustomer(p.TargetId).Name,
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priority)p.Priority,
                    Status = GetParcelStatus(p.Scheduled, p.PickedUp, p.Delivered),
                }) ;
            }
            return tmp;
        }
        #endregion
        #region get single element  
        public BaseStation GetBaseStation(int id)
        {
            IDAL.DO.BaseStation st;
            try
            {
                st = myDal.GetBaseStation(id);
            }
            catch (IDAL.BaseStationExceptionDAL ex)
            {
                throw new BaseStationException("Base Station Exception: ", ex);
            }
            return new BaseStation
            {
                Id = st.Id,
                Name = st.Name,
                StationLocation = CreateLocation(st.Longitude, st.Lattitude),
                NumOfSlots = st.NumOfSlots,
                DronesCharging = GetAllDroneCharges(id),
            };
        }
        public Drone GetDrone(int id)
        {
            IDAL.DO.Drone dr;
            try
            {
                dr = myDal.GetDrone(id);
            }
            catch (IDAL.DroneExceptionDAL ex)
            {
                throw new DroneException("Drone Exception: ", ex);
            }
            Drone tmp = null;
            foreach (DroneInList drone in Drones)
            {
                if (drone.Id == id)
                {
                    if (myDal.GetAllParcels().Any(prc => prc.DroneId == drone.Id))
                    {
                        tmp = new Drone
                        {
                            Id = drone.Id,
                            Model = drone.Model,
                            MaxWeight = drone.MaxWeight,
                            Status = drone.Status,
                            Battery = drone.Battery,
                            Parcel = GetParcelInDelivery(drone.ParcelId),  
                            Location = drone.DroneLocation,
                        };
                        break;
                    }
                    else
                    {
                        tmp = new Drone
                        {
                            Id = drone.Id,
                            Model = drone.Model,
                            MaxWeight = drone.MaxWeight,
                            Status = drone.Status,
                            Battery = drone.Battery,
                            Parcel = null,
                            Location = drone.DroneLocation,
                        };
                        break;
                    }
                }
            }
            return tmp;
        }
        public DroneInParcel GetDroneInParcel(int id)
        { 
            Drone dr = GetDrone(id);
            return new DroneInParcel
            {
                Id = dr.Id,
                Battery = dr.Battery,
                DroneLocation = dr.Location,
            };
        }
        public Customer GetCustomer(int id)
        {
            IDAL.DO.Customer cstmr;
            try
            {
                cstmr = myDal.GetCustomer(id);
            }
            catch (IDAL.CustomerExceptionDAL ex)
            {
                throw new CustomerException("Customer Exception: ", ex);
            }
            return new Customer
            {
                Id = cstmr.Id,
                Name = cstmr.Name,
                Phone = cstmr.Phone,
                CustomerLocation = CreateLocation(cstmr.Longitude, cstmr.Lattitude),
                From = GetAllOutGoingDeliveries(id),
                To = GetAllIncomingDeliveries(id),
            };
        }
        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel pr;
            try
            {
                pr = myDal.GetParcel(id);
            }
            catch (IDAL.ParcelExceptionDAL ex)
            {
                throw new ParcelException("Parcel Exception: ", ex);
            }
            Parcel tmp = null;
            if (pr.DroneId != 0)
            {
                tmp = new Parcel
                {
                    Id = pr.Id,
                    Sender = GetCustomer(pr.SenderId),
                    Target = GetCustomer(pr.TargetId),
                    Weight = (WeightCategories)pr.Weight,
                    Priority = (Priority)pr.Priority,
                    Drone = GetDroneInParcel(pr.DroneId),
                    Ordered = pr.Requested,
                    Linked = pr.Scheduled,
                    PickedUp = pr.PickedUp,
                    Delivered = pr.Delivered,
                };
            }
            else
            {
                tmp = new Parcel
                {
                    Id = pr.Id,
                    Sender = GetCustomer(pr.SenderId),
                    Target = GetCustomer(pr.TargetId),
                    Weight = (WeightCategories)pr.Weight,
                    Priority = (Priority)pr.Priority,
                    Drone = null,
                    Ordered = pr.Requested,
                    Linked = pr.Scheduled,
                    PickedUp = pr.PickedUp,
                    Delivered = pr.Delivered,
                };
            }
            return tmp;
        }
        public CustomerInParcel GetCustomerInParcel(int id)
        {
            IDAL.DO.Customer cstmr = myDal.GetCustomer(id);
            return new CustomerInParcel
            {
                Id = cstmr.Id,
                Name = cstmr.Name,
            };
        }
        public ParcelInDelivery GetParcelInDelivery(int id)
        {
            Parcel prc2 = GetParcel(id);
            bool flag = false;
            if (GetParcelStatus(prc2.Linked, prc2.PickedUp, prc2.Delivered) == ParcelStatus.PickedUp)
                flag = true;
            return new ParcelInDelivery
            {
                Id = prc2.Id,
                Status = flag,
                Priority = prc2.Priority,
                Weight = prc2.Weight,
                Sender = GetCustomerInParcel(prc2.Sender.Id),
                Target = GetCustomerInParcel(prc2.Target.Id),
                SenderLocation = prc2.Sender.CustomerLocation,
                TargetLocation = prc2.Target.CustomerLocation,
                DeliveryDistance = Distance.GetDistance(prc2.Sender.CustomerLocation, prc2.Target.CustomerLocation),
            };
        }
        #endregion
        #region get specific details
        /// <summary>
        /// Get id of nearest base station with available charging slots from 
        /// a specific location
        /// </summary>
        /// <param name="l"> location to calculate from </param>
        /// <param name="stations"> list of all base stations</param>
        /// <returns> int </returns>
        public Location GetBasestationLocation(int id)
        {
            return GetBaseStation(id).StationLocation;
        }
        public int GetNearestBasestationID(Location l, IEnumerable<BaseStation> stations)
        {
            double min = double.PositiveInfinity;
            double temp;
            int id = stations.First().Id;
            foreach (BaseStation st in stations)
            {
                temp = Distance.GetDistance(st.StationLocation, l);
                if (temp < min)
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
                if (temp < min)
                {
                    min = temp;
                    id = prc.Id;
                }
            }
            return id;
        }
        public ParcelStatus GetParcelStatus(DateTime scheduled, DateTime pickedUp, DateTime delivered)
        {
            if (scheduled == DateTime.MinValue)
                return ParcelStatus.Orderd;
            if (pickedUp == DateTime.MinValue)
                return ParcelStatus.Linked;
            if (delivered == DateTime.MinValue)
                return ParcelStatus.PickedUp;
            return ParcelStatus.Delivered;
        }
        public bool CheckDroneDistanceCoverage(Drone dr, Parcel prc, WeightCategories w)
        {
            double DroneToSender = Distance.GetDistance(dr.Location, prc.Sender.CustomerLocation) * DroneElecUseEmpty;
            double SenderToTarget = Distance.GetDistance(prc.Sender.CustomerLocation, prc.Target.CustomerLocation);
            double TargetToBaseStation = Distance.GetDistance(prc.Target.CustomerLocation, GetBasestationLocation(GetNearestBasestationID(prc.Target.CustomerLocation, GetAllAvailableBaseStations()))) * DroneElecUseEmpty;
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
        public int GetMinimalCharge(Location l, Parcel prc)
        {
            double StTOSender = Distance.GetDistance(l, prc.Sender.CustomerLocation) * DroneElecUseEmpty;
            double SenderToTarget = Distance.GetDistance(prc.Sender.CustomerLocation, prc.Target.CustomerLocation);
            Location stLocation = GetBaseStation(GetNearestBasestationID(prc.Target.CustomerLocation, GetAllBaseStations())).StationLocation;
            double TargetToSt = Distance.GetDistance(prc.Target.CustomerLocation, stLocation) * DroneElecUseEmpty;
            switch (prc.Weight)
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
            if ((int)(StTOSender + SenderToTarget + TargetToSt) > 99)
                return 75;
            return (int)(StTOSender + SenderToTarget + TargetToSt);


        }
        #endregion
        #endregion




    }
}


