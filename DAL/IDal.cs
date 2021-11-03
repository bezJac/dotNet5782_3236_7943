using System;
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
        public void AddBaseStation(BaseStation st);
        public void AddDrone(Drone dr);
        public void AddCustomer(Customer person);
        public void AddParcel(Parcel pack);
        #endregion

        #region Update part of C.R.U.d
        public void UpdateBaseStation(BaseStation bst);
        public void UpdateDrone(Drone dr);
        public void UpdateCustomer(Customer person);
        public void UpdateParcel(Parcel pack);
        public void LinkParcelToDrone(Parcel prc, Drone dr);
        public void DroneParcelPickup(Parcel prc);
        public void ParcelDelivery(Parcel prc);
        public void ChargeDrone(BaseStation bst, Drone dr);
        public void ReleaseDroneCharge(BaseStation bst, Drone dr);
        #endregion

        #region Delete part of C.R.U.D
        public void RemoveBaseStation(BaseStation bst);
        public void RemoveDrone(Drone dr);
        public void RemoveCustomer(Customer person);
        public void RemoveParcel(Parcel pack);
        #endregion

        #region Read part of C.R.U.D
        #region Read all elements
        public IEnumerable<BaseStation> GetAllBaseStations();
        public IEnumerable<Drone> GetAllDrones();
        public IEnumerable<Customer> GetAllCustomers();
        public IEnumerable<Parcel> GetAllParcels();
        public IEnumerable<BaseStation> GetAvailableCharge();
        public IEnumerable<Parcel> GetUnlinkedParcels();
        #endregion
        #region Read single element  
        public BaseStation GetBaseStation(int id);
        public Drone GetDrone(int id);
        public Customer GetCustomer(int id);
        public Parcel GetParcel(int id);
        #endregion
        public IEnumerable<double> GetElectricUse();
        #endregion

     
        public double GetDistance(double longt, double latt, BaseStation station);
        public double GetDistance(double longt, double latt, Customer cstmr);
        public double DistanceCalc(double lat1, double lon1, double lat2, double lon2);
        



    }
}
