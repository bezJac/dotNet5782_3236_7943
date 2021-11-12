﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace IDAL
{
    public interface IDal
    {
        #region Create part of C.R.U.D
        void AddBaseStation(BaseStation st);
        void AddDrone(Drone dr);
        void AddCustomer(Customer person);
        void AddParcel(Parcel pack);
        void AddDroneCharge(DroneCharge dc);
        #endregion

        #region Update part of C.R.U.d
        void UpdateBaseStation(BaseStation bst);
        void UpdateDrone(Drone dr);
        void UpdateCustomer(Customer person);
        void UpdateParcel(Parcel pack);
        #endregion

        #region Delete part of C.R.U.D
        void RemoveBaseStation(BaseStation bst);
        void RemoveDrone(Drone dr);
        void RemoveCustomer(Customer person);
        void RemoveParcel(Parcel pack);
        void RemoveDroneCharge(DroneCharge dc);
        #endregion

        #region Read part of C.R.U.D
        #region Read list of elements
        IEnumerable<BaseStation> GetAllBaseStations(Predicate<BaseStation> predicate = null);
        IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null);
        IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null);
        IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null);
        IEnumerable<DroneCharge> GetAllDronecharges(Predicate<DroneCharge> predicate = null);
        #endregion
        #region Read single element  
        BaseStation GetBaseStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);
        DroneCharge GetDroneCharge(int droneId);
        #endregion
        #region Read specific details
        IEnumerable<double> GetElectricUse();
        #endregion
        #endregion

     
        
        



    }
}
