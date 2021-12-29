using DO;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DalXml : DalApi.IDal
    {
        public void AddBaseStation(BaseStation st)
        {
            throw new NotImplementedException();
        }

        public void AddCustomer(Customer person)
        {
            throw new NotImplementedException();
        }

        public void AddDrone(Drone dr)
        {
            throw new NotImplementedException();
        }

        public void AddDroneCharge(DroneCharge dc)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel pack)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStation> GetAllBaseStations(Func<BaseStation, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAllCustomers(Func<Customer, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetAllDronecharges(Func<DroneCharge, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetAllDrones(Func<Drone, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetAllParcels(Func<Parcel, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public BaseStation GetBaseStation(int id)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int id)
        {
            throw new NotImplementedException();
        }

        public DroneCharge GetDroneCharge(int droneId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<double> GetElectricUse()
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveBaseStation(BaseStation bst)
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomer(Customer person)
        {
            throw new NotImplementedException();
        }

        public void RemoveDrone(Drone dr)
        {
            throw new NotImplementedException();
        }

        public void RemoveDroneCharge(DroneCharge dc)
        {
            throw new NotImplementedException();
        }

        public void RemoveParcel(Parcel pack)
        {
            throw new NotImplementedException();
        }

        public void UpdateBaseStation(BaseStation bst)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Customer person)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrone(Drone dr)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcel(Parcel pack)
        {
            throw new NotImplementedException();
        }
    }
}
