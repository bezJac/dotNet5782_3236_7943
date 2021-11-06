using BL.BO;
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

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
           foreach(IDAL.DO.Drone dr in myDal.GetAllDrones())
            {
                

            }
                    
        }

        public void AddBaseStation(BaseStation station)
        {
            if(myDal.GetAllBaseStations().Any(st => st.Id == station.Id))
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
        public void AddDrone(Drone drone , int stationId)
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
                MaxWeight=(IDAL.DO.WeightCategories)drone.MaxWeight,

            });
        }
        public void AddParcel(Parcel parcel )
        {
            if (myDal.GetAllParcels().Any(pr => pr.Id == parcel.Id))
                throw new ParcelException($" {parcel.Id} exist already");
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
                Id = parcel.Id,
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                DroneId = parcel.Drone.Id,
                Requested=parcel.Ordered,
                Scheduled=parcel.Linked,
                PickedUp=parcel.PickedUp,
                Delivered=parcel.Delivered,

            }) ;
        }
        public void AddCustomer(Customer customer)
        {
            if (myDal.GetAllCustomers().Any(cst => cst.Id == customer.Id))
                throw new CustomerException($" {customer.Id} exist already");
            myDal.AddCustomer(new IDAL.DO.Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone=customer.Phone,
                Longitude=customer.CustomerLocation.Longtitude,
                Lattitude=customer.CustomerLocation.Lattitude,

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
        public IEnumerable<DroneCharge> GetAllDroneCharges()
        {
            return myDal.GetAllDronecharges().Select(Charge => new DroneCharge { Id = Charge.DroneId, Battery = GetDrone(Charge.DroneId).Battery });
        }
        public IEnumerable<DeliveryAtCustomer> GetAllOutGoingDeliveries(int senderId)
        {
            var deliveris = myDal.GetAllParcels()
                .Where(p => p.SenderId == senderId)
                .Select(parcel =>
                    new DeliveryAtCustomer
                    {
                        Id = parcel.Id,
                        Weight = (WeightCategories)parcel.Weight,
                        Priority = (Priority)parcel.Priority,
                        Status = (ParcelStatus)GetParcelStatusIndicator(parcel.Id),
                        CounterCustomer = GetCustomerDetails(parcel.TargetId),

                    });
            return deliveris;
        }
        public IEnumerable<DeliveryAtCustomer> GetAllIncomingDeliveries(int targetId)
        {
            var deliveris = myDal.GetAllParcels()
                .Where(p => p.TargetId == targetId)
                .Select(parcel =>
                    new DeliveryAtCustomer
                    {
                        Id = parcel.Id,
                        Weight = (WeightCategories)parcel.Weight,
                        Priority = (Priority)parcel.Priority,
                        Status = (ParcelStatus)GetParcelStatusIndicator(parcel.Id),
                        CounterCustomer = GetCustomerDetails(parcel.SenderId),

                    });
            return deliveris;
        }
        public IEnumerable<BaseStation> GetAllAvailableBAseStations()
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

        public BaseStation GetBaseStation(int id)
        {
            if (!myDal.GetAllBaseStations().Any(b => b.Id == id))
                throw new BaseStationException($"base station -  {id} dosen't exist ");
            BaseStation temp = null;
            foreach (IDAL.DO.BaseStation st in myDal.GetAllBaseStations())
            {
                if(st.Id == id)
                {
                    Location l =  new Location(); ;
                    l.Longtitude = st.Longitude;
                    l.Lattitude = st.Lattitude;
                    temp = new BaseStation
                    {
                        Id = st.Id,
                        Name = st.Name,
                        StationLocation = l,
                        NumOfSlots = st.NumOfSlots,
                        DronesCharging = GetAllDroneCharges(),
                    };
                }
            }
            if (temp == null)
                throw new BaseStationException($"base station - {id} wasen't found");
            return temp;
        }
        public Drone GetDrone(int id)
        {
            Drone temp = null;
            foreach (DroneInList dr in Drones)
            {
                if (dr.Id == id)
                {
                    temp = new Drone
                    {
                        Id = dr.Id,
                        Model = dr.Model,
                        MaxWeight = dr.MaxWeight,
                        Status = dr.Status,
                        Battery = dr.Battery,
                        Parcel = GetDelivery(dr.ParcelId),
                        Location = dr.DroneLocation,

                    };
                    break;
                }

            }
            if (temp == null)
            {
                throw new DroneException("id not found");
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
                        To = GetAllOutGoingDeliveries(customer.Id),

                    };
                }
            }
            return temp;

        }
        public Parcel GetParcel(int id)
        {
            if (!myDal.GetAllParcels().Any(p => p.Id == id))
                throw new CustomerException($"parcel -  {id} dosen't exist ");
            var parcel = myDal.GetAllParcels()
                .Where(p => p.Id == id)
                .Select(prc =>
                    new Parcel
                    {
                        Id = prc.Id,
                        Sender = GetCustomer(prc.SenderId),
                        Target  = GetCustomer(prc.TargetId),
                        Weight =(WeightCategories)prc.Weight,
                        Priority = (Priority)prc.Priority,
                        Drone = GetDrone(prc.DroneId),
                        Ordered = prc.Requested,
                        Linked = prc.Scheduled,
                        PickedUp = prc.PickedUp,
                        Delivered = prc.Delivered,
                    });
            return (Parcel)parcel;
        }
        public CustomerDelivery GetCustomerDetails(int id)
        {
            var customer = myDal.GetAllCustomers()
                .Where(c => c.Id == id)
                .Select(cstmr =>
                    new CustomerDelivery
                    {
                        Id = myDal.GetCustomer(id).Id,
                        Name = myDal.GetCustomer(id).Name,
                    });
            return (CustomerDelivery)customer;
        }


        public Delivery GetDelivery(int id)
        {
            throw new ParcelException();
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
            int stationId = GetNearestBasestationID(Drones[index].DroneLocation, GetAllBaseStations());
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

        // extra functions
        /// <summary>
        /// Get id of nearest base station with available charging slots from 
        /// a specific location
        /// </summary>
        /// <param name="l"> location to calculate from </param>
        /// <param name="stations"> list of all base stations</param>
        /// <returns> int </returns>
        public int GetNearestBasestationID(Location l, IEnumerable<BaseStation> stations)
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


    }
    
}
