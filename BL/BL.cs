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
                            Battery = 99,//rnd.Next(tempBattery, 101),
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
                throw new BaseStationException("BL: ", ex);
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
                throw new BaseStationException("BL: ", Ex);
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
                DroneLocation = createLocation(st.Longitude, st.Lattitude),
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
                throw new ParcelException("BL: ", Ex);
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
                throw new CustomerException("BL: ", Ex);
            }
        }
        #endregion
        #region Updating a Entity methods 
        #region user specific entity static details update 
        public void UpdateDrone(int id, string model)
        {
            IDAL.DO.Drone dr;
            try
            {
                dr = myDal.GetDrone(id);
            }
            catch (IDAL.DroneExceptionDAL Ex)
            {
                throw new DroneException("BL: ", Ex);
            }
            dr.Model = model;
            myDal.UpdateDrone(dr);
            int index = Drones.FindIndex(dr => dr.Id == id);
            Drones[index].Model = model;
        }
        public void UpdateBaseStation(int id, int count, string name)
        {
            BaseStation station = GetBaseStation(id);
            if (station.DronesCharging.Count() > count)
                throw new BaseStationException($"base station: {id} Occupied slots exceed requested update");
            IDAL.DO.BaseStation st=new IDAL.DO.BaseStation();
            st.Id = id;
            if (name != "")
                st.Name = name;
            else
                st.Name = station.Name;
            st.Longitude = station.StationLocation.Longtitude;
            st.Lattitude = station.StationLocation.Lattitude;
            if (count == 0)
                st.NumOfSlots = station.NumOfSlots;
            else
                st.NumOfSlots = count - station.DronesCharging.Count();
            myDal.UpdateBaseStation(st) ;
        }
        public void UpdateCustomer(int id, string phone, string name)
        {
            IDAL.DO.Customer cstmr;
            try
            {
                cstmr = myDal.GetCustomer(id);
            }
            catch (IDAL.CustomerExceptionDAL Ex)
            {
                throw new CustomerException ("BL: ",Ex);
            }
            if( name!="")
            cstmr.Name = name;
            if(phone!="")
            cstmr.Phone = phone;
            myDal.UpdateCustomer(cstmr);
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

            IDAL.DO.BaseStation st = getNearestBasestation(Drones[index].DroneLocation);
            Location stLocation = createLocation(st.Longitude, st.Lattitude);
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
            DroneInList dr = GetAllDronesInList().ToList().Find(dr => dr.Id == id);
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
                            IDAL.DO.Parcel prc = getNearestParcel(dr.DroneLocation, temp);
                            IDAL.DO.Customer sender = myDal.GetCustomer(prc.SenderId);
                            IDAL.DO.Customer target = myDal.GetCustomer(prc.TargetId);
                            if (checkDroneDistanceCoverage(dr, createLocation(sender.Longitude, sender.Lattitude), createLocation(target.Longitude, target.Longitude), dr.MaxWeight))
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

            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.TargetLocation) * getElectricUseForDrone(prc.Weight));
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
            IEnumerable<IDAL.DO.BaseStation> stations;
            try
            {
                stations = myDal.GetAllBaseStations();
            }
            catch (IDAL.BaseStationExceptionDAL Ex)
            {
                throw new BaseStationException("BL: ", Ex);
            }
            return stations.Select(st => convertToBaseStation(st));
        }
        public IEnumerable<BaseStationInList> GetALLBaseStationInList()
        {
            IEnumerable<BaseStation> stations;
            try
            {
                stations = GetAllBaseStations();
            }
            catch (BaseStationException ex)
            {
                throw new BaseStationException("BL: ", ex);
            }
            return stations.Select(st => new BaseStationInList
            {
                Id = st.Id,
                Name = st.Name,
                AvailableSlots = st.NumOfSlots,
                OccupiedSlots = st.DronesCharging.Count(),
            });
        }
        public IEnumerable<BaseStationInList> GetAllAvailablBaseStations()
        {
            IEnumerable<IDAL.DO.BaseStation> stations;
            try
            {
                stations = myDal.GetAllBaseStations(st => st.NumOfSlots > 0);
            }
            catch (BaseStationException ex)
            {
                throw new BaseStationException("BL: ", ex);
            }
            return stations.Select(st => convertToBaseStation(st)).Select(st => new BaseStationInList
            {
                Id = st.Id,
                Name = st.Name,
                AvailableSlots = st.NumOfSlots,
                OccupiedSlots = st.DronesCharging.Count(),
            });
        }
        public IEnumerable<Drone> GetAllDrones()
        {
            if (Drones.Count() <= 0)
                throw new DroneException("no drones in list");
            return Drones.Select(dr => convertToDrone(dr));
        }
        public IEnumerable<DroneInList> GetAllDronesInList()
        {
            if (Drones.Count() > 0)
                return Drones.ToList();
            throw new DroneException("no drones in list");
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            IEnumerable<IDAL.DO.Customer> customers;
            try
            {
                customers = myDal.GetAllCustomers();
            }
            catch (IDAL.CustomerExceptionDAL ex)
            {
                throw new CustomerException("Bl: ", ex);
            }
            return customers.Select(cstmr => convertToCustomer(cstmr));
        }
        public IEnumerable<CustomerInList> GetAllCustomersInList()
        {
            IEnumerable<Customer> customers = GetAllCustomers();
            List<CustomerInList> tmp = new List<CustomerInList>();
            foreach (Customer cs in customers)
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
        public IEnumerable<Parcel> GetAllParcels()
        {
            IEnumerable<IDAL.DO.Parcel> parcels;
            try
            {
                parcels = myDal.GetAllParcels();
            }
            catch (IDAL.ParcelExceptionDAL ex)
            {
                throw new ParcelException("Bl: ", ex);
            }
            return parcels.Select(prc => convertToParcel(prc));
        }
        public IEnumerable<ParcelInList> GetAllParcelsInList()
        {
            IEnumerable<IDAL.DO.Parcel> parcels;
            try
            {
                parcels = myDal.GetAllParcels();
            }
            catch (IDAL.ParcelExceptionDAL ex)
            {
                throw new ParcelException("Bl: ", ex);
            }
            return parcels.Select(prc => convertToParcelInList(prc));
        }
        public IEnumerable<ParcelInList> GetAllUnlinkedParcels()
        {
            IEnumerable<IDAL.DO.Parcel> parcels;
            try
            {
                parcels = myDal.GetAllParcels(prc => prc.DroneId == 0);
            }
            catch (IDAL.ParcelExceptionDAL ex)
            {
                throw new ParcelException("Bl: ", ex);
            }
            return parcels.Select(prc => convertToParcelInList(prc));
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
                        Status = getParcelStatus(parcel),
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
                        Status = getParcelStatus(parcel),
                        CounterCustomer = GetCustomerInParcel(parcel.SenderId),

                    });
            return deliveris;
        }
        public IEnumerable<DroneCharge> GetAllDronesCharging(int stationId)
        {
            return myDal.GetAllDronecharges(st => st.StationId == stationId)
            .Select(Charge => new DroneCharge
            {
                Id = Charge.DroneId,
                Battery = GetDrone(Charge.DroneId).Battery
            });
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
            return convertToBaseStation(st);
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
            return convertToDrone(Drones.Find(drone => drone.Id == dr.Id));
        }
        public DroneInParcel GetDroneInParcel(int id)
        {
            DroneInList dr = Drones.Find(dr => dr.Id == id);
            return new DroneInParcel
            {
                Id = dr.Id,
                Battery = dr.Battery,
                DroneLocation = dr.DroneLocation,
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
            return convertToCustomer(cstmr);
        }
        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel prc;
            try
            {
                prc = myDal.GetParcel(id);
            }
            catch (IDAL.ParcelExceptionDAL ex)
            {
                throw new ParcelException("Parcel Exception: ", ex);
            }
            return convertToParcel(prc);
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
            IDAL.DO.Parcel parcel;
            IDAL.DO.Customer sender;
            IDAL.DO.Customer target;
            try
            {
                parcel = myDal.GetParcel(id);
                sender = myDal.GetCustomer(parcel.SenderId);
                target = myDal.GetCustomer(parcel.TargetId);
            }
            catch (IDAL.ParcelExceptionDAL ex)
            {

                throw new ParcelException("BL - Parcel Exception: ", ex);
            }
            catch (IDAL.CustomerExceptionDAL ex)
            {

                throw new CustomerException("BL - CustomerException: ", ex);
            }
            bool flag = false;
            if (getParcelStatus(parcel) == ParcelStatus.PickedUp)
                flag = true;
            Location senderLocation = createLocation(sender.Longitude, sender.Lattitude);
            Location targetLocation = createLocation(target.Longitude, target.Lattitude);
            return new ParcelInDelivery
            {
                Id = parcel.Id,
                Status = flag,
                Priority = (Priority)parcel.Priority,
                Weight = (WeightCategories)parcel.Weight,
                Sender = GetCustomerInParcel(parcel.SenderId),
                Target = GetCustomerInParcel(parcel.TargetId),
                SenderLocation = senderLocation,
                TargetLocation = targetLocation,
                DeliveryDistance = Distance.GetDistance(senderLocation, targetLocation),
            };

        }
        #endregion
        #endregion
        #region private methods for local calculations
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


