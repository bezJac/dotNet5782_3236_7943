﻿using BL.BO;
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
                    Location senderLocation = CreateLocation(sender.Longitude, sender.Lattitude);
                    Location targetLocation = CreateLocation(target.Longitude, target.Lattitude);
                    IDAL.DO.BaseStation charge = GetNearestBasestation(targetLocation);
                    Location station = CreateLocation(charge.Longitude,charge.Lattitude);
                    if (parcel.PickedUp == DateTime.MinValue)
                    {
                        IDAL.DO.BaseStation st = GetNearestBasestation(senderLocation);
                        Location current = CreateLocation(st.Longitude, st.Lattitude);
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Delivery,
                            Battery = rnd.Next(GetMinimalCharge(current, senderLocation, targetLocation, station, (WeightCategories)parcel.Weight), 101),
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
                            Battery = rnd.Next(GetMinimalCharge(senderLocation, senderLocation, targetLocation, station, (WeightCategories)parcel.Weight), 101),
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
                        IDAL.DO.BaseStation st = GetNearestBasestation(current);
                        int tempBattery = (int)(Distance.GetDistance(current, CreateLocation(st.Longitude,st.Lattitude)) * DroneElecUseEmpty);
                        Drones.Add(new DroneInList
                        {
                            Id = dr.Id,
                            Model = dr.Model,
                            MaxWeight = (WeightCategories)dr.MaxWeight,
                            Status = DroneStatus.Available,
                            Battery = 99,//rnd.Next(tempBattery, 101),
                            ParcelId = 0,
                            DroneLocation = current,
                        }) ;
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
                            DroneLocation = CreateLocation(tempSt.ElementAt(index).Longitude, tempSt.ElementAt(index).Lattitude),
                        });
                        IDAL.DO.BaseStation st = tempSt.ElementAt(index);
                        st.NumOfSlots--;
                        myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = dr.Id, StationId = st.Id });
                        myDal.UpdateBaseStation(st);
                        
                    }
                }
            }
        }

        #region Adding new entity methods
        public void AddBaseStation(BaseStation station)
        {
            try
            {
                myDal.AddBaseStation(new IDAL.DO.BaseStation
                {
                    Id = station.Id,
                    Name = station.Name,
                    Longitude = station.StationLocation.Longtitude,
                    Lattitude = station.StationLocation.Lattitude,
                    NumOfSlots = station.NumOfSlots,
                });
            }
            catch (IDAL.BaseStationExceptionDAL ex)
            {
                throw new BaseStationException("BL: ",ex);
            }
        }
        public void AddDrone(Drone drone, int stationId)
        {
            IDAL.DO.BaseStation st;
            try
            {
                st = myDal.GetBaseStation(stationId);
                if (st.NumOfSlots == 0)
                    throw new BaseStationException($"base station - {stationId} has no charging slots available");
                myDal.AddDrone(new IDAL.DO.Drone
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,
                });
            }
            catch (IDAL.BaseStationExceptionDAL Ex)
            {
                throw new BaseStationException("BL: ",Ex);
            }
            catch (IDAL.DroneExceptionDAL Ex)
            {
                throw new DroneException("BL: ", Ex);
            }
            
            Random rnd = new Random();
            Drones.Add(new DroneInList
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Status = DroneStatus.Maintenance,
                Battery = rnd.Next(20, 41),
                DroneLocation = CreateLocation(st.Longitude,st.Lattitude),
            });
            st.NumOfSlots--;
            myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = drone.Id, StationId = st.Id });
            myDal.UpdateBaseStation(st);
        }
        public void AddParcel(Parcel parcel)
        {
            try
            {
                myDal.GetCustomer(parcel.Sender.Id);
                myDal.GetCustomer(parcel.Target.Id);
            }
            catch (IDAL.CustomerExceptionDAL Ex)
            {
                throw new ParcelException("BL: ",Ex);
            }
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
            try
            {
                myDal.AddCustomer(new IDAL.DO.Customer
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Longitude = customer.CustomerLocation.Longtitude,
                    Lattitude = customer.CustomerLocation.Lattitude,

                });
            }
            catch (IDAL.CustomerExceptionDAL Ex)
            {
                throw new CustomerException("BL: ",Ex);
            }
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
           
            IDAL.DO.BaseStation st = GetNearestBasestation(Drones[index].DroneLocation);
            Console.WriteLine("STATION CHARGING "+ st.Id);
            Location stLocation = CreateLocation(st.Longitude, st.Lattitude);
            double distance = Distance.GetDistance(stLocation, Drones[index].DroneLocation);

            // check if drone has enough battery to cover the  distance
            if ((Drones[index].Battery - distance * DroneElecUseEmpty) <= 0)
                throw new DroneException($"drone - {id} does not have enough battery to reach nearest base station");

            // update drone's details 
            Drones[index].Battery -= (int)(distance * DroneElecUseEmpty);
            Drones[index].DroneLocation = stLocation;
            Drones[index].Status = DroneStatus.Maintenance;

            // update necessary details in datasource
            myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = id, StationId = st.Id });
            st.NumOfSlots--;
            myDal.UpdateBaseStation(st);
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

            IDAL.DO.DroneCharge tempCharge = myDal.GetDroneCharge(id);
            IDAL.DO.BaseStation tempSt = myDal.GetBaseStation(tempCharge.StationId);
            myDal.RemoveDroneCharge(tempCharge);
            tempSt.NumOfSlots++;
            myDal.UpdateBaseStation(tempSt);
        }
        public void LinkDroneToParcel(int id)
        {
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            List<IDAL.DO.Parcel> unlinked;
            try
            {
                unlinked = myDal.GetAllParcels(pr => pr.DroneId == 0).ToList();
            }
            catch (IDAL.ParcelExceptionDAL ex)
            {
                throw new ParcelException("BL - ", ex);
            }
            DroneInList dr  = GetAllDronesInList().ToList().Find(dr => dr.Id == id);
            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            for (int i = (int)Priority.Emergency; i > 0; i--)
            {
                List<IDAL.DO.Parcel> temp = unlinked.FindAll(prc => (Priority)prc.Priority == (Priority)i);
                if (temp.Count > 0)
                {
                    for (int j = (int)dr.MaxWeight; j > 0; j--)
                    {
                        temp = temp.FindAll(prc => (WeightCategories)prc.Weight == (WeightCategories)j);
                        if (temp.Count() > 0)
                        {
                            IDAL.DO.Customer sender = new IDAL.DO.Customer();
                            IDAL.DO.Customer target = new IDAL.DO.Customer();
                            IDAL.DO.Parcel prc = GetNearestParcel(dr.DroneLocation,ref sender,ref target, temp);
                         
                            if (CheckDroneDistanceCoverage(dr, CreateLocation(sender.Longitude,sender.Lattitude),CreateLocation(target.Longitude,target.Longitude), dr.MaxWeight))
                            {
                                prc.DroneId = id;
                                prc.Scheduled = DateTime.Now;
                                myDal.UpdateParcel(prc);
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
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            Drone dr = GetDrone(id);
            if (dr.Parcel == null)
                throw new DroneException($"Drone - {id} isen't linked yet to a parcel");
            if ((dr.Parcel.Status == true))
                throw new DroneException($"Drone - {id} already picked up parcel");

            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.SenderLocation) * DroneElecUseEmpty);
            Drones[index].DroneLocation = dr.Parcel.SenderLocation;

            IDAL.DO.Parcel p = myDal.GetParcel(dr.Parcel.Id);
            p.PickedUp = DateTime.Now;
            myDal.UpdateParcel(p);
        }
        public void DroneParcelDelivery(int id)
        {
            if (!Drones.Any(dr => dr.Id == id))
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

            IDAL.DO.Parcel tempPrc = myDal.GetParcel(prc.Id);
            tempPrc.Delivered = DateTime.Now;
            tempPrc.DroneId = 0;
            myDal.UpdateParcel(tempPrc);
            
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
            List<Drone> temp = new List<Drone>();
            foreach (DroneInList dr in Drones)
            {
                temp.Add(GetDrone(dr.Id));
            }
            return temp;
        }
        public IEnumerable<DroneInList> GetAllDronesInList()
        {
            return Drones.ToList();
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
                        Status = GetParcelStatus(parcel),
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
                        Status = GetParcelStatus(parcel),
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
                    Status = GetParcelStatus(p),
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
                    Sender = GetCustomerInParcel(pr.SenderId),
                    Target = GetCustomerInParcel(pr.TargetId),
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
                    Sender = GetCustomerInParcel(pr.SenderId),
                    Target = GetCustomerInParcel(pr.TargetId),
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
            IDAL.DO.Parcel tmp;
            IDAL.DO.Customer sender;
            IDAL.DO.Customer target;
            try
            {
                tmp = myDal.GetParcel(id);
                sender = myDal.GetCustomer(tmp.SenderId);
                target = myDal.GetCustomer(tmp.TargetId);
            }
            catch (IDAL.ParcelExceptionDAL ex )
            {

                throw new ParcelException("BL - Parcel Exception: ", ex);
            }
            catch (IDAL.CustomerExceptionDAL ex)
            {

                throw new CustomerException("BL - CustomerException: ", ex);
            }
            bool flag = false;
            if (GetParcelStatus(tmp) == ParcelStatus.PickedUp)
                flag = true;
            Location senderLocation = CreateLocation(sender.Longitude, sender.Lattitude);
            Location targetLocation = CreateLocation(target.Longitude, target.Lattitude);         
            return new ParcelInDelivery
            {
                Id = tmp.Id,
                Status = flag,
                Priority = (Priority)tmp.Priority,
                Weight = (WeightCategories)tmp.Weight,
                Sender = GetCustomerInParcel(tmp.SenderId),
                Target = GetCustomerInParcel(tmp.TargetId),
                SenderLocation = senderLocation,
                TargetLocation = targetLocation,
                DeliveryDistance = Distance.GetDistance(senderLocation,targetLocation),
            };
        }
        #endregion
        #region get specific details
        private IDAL.DO.BaseStation GetNearestBasestation(Location l )
        {
            double min = double.PositiveInfinity;
            double temp;
            int id=0;
            foreach (IDAL.DO.BaseStation st in myDal.GetAllBaseStations(s=>s.NumOfSlots>0))
            {
                temp = Distance.GetDistance(CreateLocation(st.Longitude,st.Lattitude), l);
                if (temp < min)
                {
                    min = temp;
                    id = st.Id;
                }
            }
            return myDal.GetBaseStation(id);
        }
        private IDAL.DO.Parcel GetNearestParcel(Location l, ref IDAL.DO.Customer sender, ref IDAL.DO.Customer target, IEnumerable<IDAL.DO.Parcel> parcels)
        {

            double min = double.PositiveInfinity;
            double temp;
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
            foreach (IDAL.DO.Parcel prc in parcels)
            {
                IDAL.DO.Customer cs = myDal.GetCustomer(prc.SenderId);
                temp = Distance.GetDistance(CreateLocation(sender.Longitude,sender.Lattitude), l);
                if (temp < min)
                {
                    min = temp;
                    parcel = prc;
                    sender = cs;
                }
            }
            target = myDal.GetCustomer(parcel.TargetId);
            return parcel;
        }
        private ParcelStatus GetParcelStatus(IDAL.DO.Parcel pr)
        {
            if (pr.Scheduled == DateTime.MinValue)
                return ParcelStatus.Orderd;
            if (pr.PickedUp == DateTime.MinValue)
                return ParcelStatus.Linked;
            if (pr.Delivered == DateTime.MinValue)
                return ParcelStatus.PickedUp;
            return ParcelStatus.Delivered;
        }
        private bool CheckDroneDistanceCoverage(DroneInList dr, Location sender, Location target, WeightCategories w)
        {
            IDAL.DO.BaseStation tmp = GetNearestBasestation(target);
            double DroneToSender = Distance.GetDistance(dr.DroneLocation, sender) * DroneElecUseEmpty;
            double SenderToTarget = Distance.GetDistance(sender, target);
            double TargetToBaseStation = Distance.GetDistance(target,CreateLocation(tmp.Longitude,tmp.Lattitude)) * DroneElecUseEmpty;
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
        private double GetElectricUseForDrone(WeightCategories w)
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
        private int GetMinimalCharge(Location current,Location sender, Location target,Location station ,WeightCategories w)
        {
            double currToSender = Distance.GetDistance(current, sender) * DroneElecUseEmpty;
            double SenderToTarget = Distance.GetDistance(sender ,target);
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
        private Location CreateLocation(double lon, double lat)
        {
            return new Location { Longtitude = lon, Lattitude = lat };
        }
        #endregion
        #endregion




    }
}


