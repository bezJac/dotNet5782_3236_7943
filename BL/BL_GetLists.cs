using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    /// <summary>
    /// partial class manages all get lists related methods for BL
    /// </summary>
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
                throw new GetListException("", Ex);
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
            catch (Exception Ex)
            {
                throw new GetListException("", Ex);
            }
            return stations.Select(st => convertToBaseStationInList(st));
            
        }
        public IEnumerable<BaseStationInList> GetAllAvailablBaseStations()
        {
            IEnumerable<IDAL.DO.BaseStation> stations;
            try
            {
                stations = myDal.GetAllBaseStations(st => st.NumOfSlots > 0);
            }
            catch (Exception ex)
            {
                throw new GetListException("", ex);
            }
            return stations.Select(st => convertToBaseStation(st)).Select(st => convertToBaseStationInList(st));
        }
        public IEnumerable<Drone> GetAllDrones()
        {
            if (drones.Count() <= 0)
                throw new GetListException("no drones in list");
            return drones.Select(dr => convertToDrone(dr));
        }
        public IEnumerable<DroneInList> GetAllDronesInList()
        {
            if (drones.Count() > 0)
                return drones.ToList();
            throw new GetListException("no drones in list");
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
                throw new GetListException("", ex);
            }
            return customers.Select(cstmr => convertToCustomer(cstmr));
        }
        public IEnumerable<CustomerInList> GetAllCustomersInList()
        {

            IEnumerable<Customer> customers;
            try
            {
               customers = GetAllCustomers();
            }
            catch (Exception ex)
            {
                throw new GetListException("", ex);
            }
            return customers.Select(cs => convertToCustomerInList(cs));
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
                throw new GetListException("", ex);
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
                throw new GetListException("", ex);
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
                throw new GetListException("", ex);
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
            return myDal.GetAllParcels()
                .Where(p => p.TargetId == targetId)
                .Select(parcel =>  new ParcelAtCustomer
                {
                    Id = parcel.Id,
                    Weight = (WeightCategories)parcel.Weight,
                    Priority = (Priority)parcel.Priority,
                    Status = getParcelStatus(parcel),
                    CounterCustomer = GetCustomerInParcel(parcel.SenderId),

                });                    
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
