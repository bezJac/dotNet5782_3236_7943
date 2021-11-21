using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using IBL.BO;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            IEnumerable<IDAL.DO.BaseStation> stations;
            try
            {
                stations = myDal.GetAllBaseStations();
            }
            catch (Exception Ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
    }
}
