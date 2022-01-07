using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            IEnumerable<DO.BaseStation> stations;
            lock (myDal)
            {
                try
                {
                    stations = myDal.GetAllBaseStations();
                }
                catch (Exception Ex)
                {
                    throw new GetListException("", Ex);
                }
                return from station in stations
                       let st = convertToBaseStation(station)
                       select st;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationInList> GetALLBaseStationInList()
        {
            IEnumerable<BaseStation> stations;
            lock (myDal)
            {
                try
                {
                    ///
                    stations = GetAllBaseStations();
                }
                catch (Exception Ex)
                {
                    throw new GetListException("", Ex);
                }
                return from station in stations
                       let st = convertToBaseStationInList(station)
                       select st;
            }

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationInList> GetAllAvailablBaseStations()
        {
            IEnumerable<DO.BaseStation> stations;
            lock (myDal)
            {
                try
                {
                    stations = myDal.GetAllBaseStations(st => st.NumOfSlots > 0);
                }
                catch (Exception ex)
                {
                    throw new GetListException("", ex);
                }
                return from station in stations
                       let st = convertToBaseStation(station)
                       let listSt = convertToBaseStationInList(st)
                       select listSt;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDrones()
        {
            if (drones.Count <= 0)
                throw new GetListException("no drones in list");
            lock (myDal)
            {
                return from dr in drones
                       let drone = convertToDrone(dr)
                       select drone;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneInList> GetAllDronesInList(DroneStatus? status = null, WeightCategories? weight = null)
        {
            if (status == null && weight == null)
            {
                if (drones.Count > 0)
                    return drones;
                throw new GetListException("No drones in list");
            }
            IEnumerable<DroneInList> tmp;
            if (status != null && weight != null)
            {
                tmp = from dr in drones
                      where dr.MaxWeight == weight && dr.Status == status
                      select dr;
                if (!tmp.Any())
                    throw new GetListException("No drones in list match filtering choice");
                return tmp;
            }
            if (status != null)
            {
                tmp = from dr in drones
                      where dr.Status == status
                      select dr;
                if (!tmp.Any())
                    throw new GetListException("No drones in list match filtering choice");
                return tmp;
            }
            if (weight != null)
            {
                tmp = from dr in drones
                      where dr.MaxWeight == weight
                      select dr;
                if (!tmp.Any())
                    throw new GetListException("No drones in list match filtering choice");
                return tmp;
            }
            throw new GetListException("No drones in list");
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomers()
        {
            IEnumerable<DO.Customer> customers;
            lock (myDal)
            {
                try
                {
                    customers = myDal.GetAllCustomers();
                }
                catch (Exception ex)
                {
                    throw new GetListException("", ex);
                }
                return from cstmr in customers
                       let cs = convertToCustomer(cstmr)
                       select cs;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerInList> GetAllCustomersInList()
        {

            IEnumerable<Customer> customers;
            lock (myDal)
            {
                try
                {
                    customers = GetAllCustomers();
                }
                catch (Exception ex)
                {
                    throw new GetListException("", ex);
                }
                return from cstmr in customers
                       let cs = convertToCustomerInList(cstmr)
                       select cs;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels()
        {
            IEnumerable<DO.Parcel> parcels;
            lock (myDal)
            {
                try
                {
                    parcels = myDal.GetAllParcels();
                }
                catch (Exception ex)
                {
                    throw new GetListException("", ex);
                }
                return from parcel in parcels
                       let prc = convertToParcel(parcel)
                       select prc;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelInList> GetAllParcelsInList(ParcelStatus? status = null, Priority? priority = null, WeightCategories? weight = null)
        {
            IEnumerable<ParcelInList> tmp;
            lock (myDal)
            {
                if (status == null && priority == null && weight == null)
                    tmp = from parcel in myDal.GetAllParcels()
                          let prc = convertToParcelInList(parcel)
                          select prc;
                else
                    tmp = from parcel in myDal.GetAllParcels()
                          where status == getParcelStatus(parcel)
                          || priority == (BO.Priority)parcel.Priority
                          || weight == (BO.WeightCategories)parcel.Weight
                          let prc = convertToParcelInList(parcel)
                          select prc;
                return !tmp.Any() ? throw new GetListException("no parcels in list match filter") : tmp;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelInList> GetAllParcelsInList(DateTime? from, DateTime? to)
        {
            lock (myDal)
            {
                IEnumerable<ParcelInList> tmp = from parcel in myDal.GetAllParcels()
                                                where parcel.Requested >= @from
                                                && parcel.Requested < to
                                                let prc = convertToParcelInList(parcel)
                                                select prc;
                return !tmp.Any() ? throw new GetListException("no parcels in list match time span") : tmp;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelInList> GetAllUnlinkedParcels()
        {
            IEnumerable<DO.Parcel> parcels;
            lock (myDal)
            {
                try
                {
                    parcels = myDal.GetAllParcels(prc => prc.DroneId == 0);
                }
                catch (Exception ex)
                {
                    throw new GetListException("", ex);
                }
                return from parcel in parcels
                       let prc = convertToParcelInList(parcel)
                       select prc;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelAtCustomer> GetAllOutGoingDeliveries(int senderId)
        {
            lock (myDal)
            {
                return from parcel in myDal.GetAllParcels()
                       where parcel.SenderId == senderId
                       select new ParcelAtCustomer
                       {
                           Id = parcel.Id,
                           Weight = (WeightCategories)parcel.Weight,
                           Priority = (Priority)parcel.Priority,
                           Status = getParcelStatus(parcel),
                           CounterCustomer = GetCustomerInParcel(parcel.TargetId),

                       };
            }

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelAtCustomer> GetAllIncomingDeliveries(int targetId)
        {
            lock (myDal)
            {
                return from parcel in myDal.GetAllParcels()
                       where parcel.TargetId == targetId
                       select new ParcelAtCustomer
                       {
                           Id = parcel.Id,
                           Weight = (WeightCategories)parcel.Weight,
                           Priority = (Priority)parcel.Priority,
                           Status = getParcelStatus(parcel),
                           CounterCustomer = GetCustomerInParcel(parcel.SenderId),

                       };
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDronesCharging(int stationId)
        {
            lock (myDal)
            {
                return from Charge in myDal.GetAllDronecharges()
                       where Charge.StationId == stationId
                       select new DroneCharge
                       {
                           Id = Charge.DroneId,
                           Battery = GetDrone(Charge.DroneId).Battery
                       };
            }
        }
    }
}