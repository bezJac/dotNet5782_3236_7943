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
        /// <summary>
        /// get copy list of all Base Stations in a BL BaseStation representation
        /// </summary>
        /// <returns> IEnumerable<BaseStation> type </returns>
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

        /// <summary>
        /// get copy list of all Base Stations in a BL BaseStationInList representation
        /// </summary>
        /// <returns> IEnumerable<BaseStationInList> type </returns>
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

        /// <summary>
        /// get copy list of all Base Stations with available charging slots in a BL BaseStationInList representation
        /// </summary>
        /// <returns> IEnumerable<BaseStationInList> type </returns>
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

        /// <summary>
        /// get copy list of all drones in a BL Drone representation
        /// </summary>
        /// <returns> IEnumerable<Drone> type </returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            if (Drones.Count() <= 0)
                throw new GetListException("no drones in list");
            return Drones.Select(dr => convertToDrone(dr));
        }

        /// <summary>
        /// get copy list of all drones in a BL DroneInList representation
        /// </summary>
        /// <returns> IEnumerable<DroneInList> type </returns>
        public IEnumerable<DroneInList> GetAllDronesInList()
        {
            if (Drones.Count() > 0)
                return Drones.ToList();
            throw new GetListException("no drones in list");
        }

        /// <summary>
        ///  get copy list of all customers in a BL Customer representation
        /// </summary>
        /// <returns> IEnumerable<Customer> type </returns>
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

        /// <summary>
        /// get copy list of all customers  in a BL CustomerInList representation
        /// </summary>
        /// <returns> IEnumerable<CustomerInList> type </returns>
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

        /// <summary>
        /// get copy list of all parcels in a BL Parcel representation
        /// </summary>
        /// <returns> IEnumerable<Parcel> type </returns>
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

        /// <summary>
        /// get copy list of all parcels in a BL ParcelInList representation
        /// </summary>
        /// <returns> IEnumerable<ParcelInList> type </returns>
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

        /// <summary>
        /// get copy list of all unlinked to drone parcels in a BL ParcelInList representation
        /// </summary>
        /// <returns> IEnumerable<ParcelInList> type </returns>
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

        /// <summary>
        /// get list of all outgoing parcels from a specific customer
        /// </summary>
        /// <param name="senderId"> sending customer ID</param>
        /// <returns> IEnumerable<ParcelAtCustomer> type </returns>
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

        /// <summary>
        /// get list of all incoming  parcels to a specific customer
        /// </summary>
        /// <param name="targetId"> target customer ID</param>
        /// <returns> IEnumerable<ParcelAtCustomer> type </returns>
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

        /// <summary>
        /// get copy list of all drone charging in a specific Base Station 
        /// </summary>
        /// <param name="stationId"> Base Station's ID</param>
        /// <returns>  IEnumerable<DroneCharge> type </returns>
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
