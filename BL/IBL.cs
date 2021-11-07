using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
     public interface IBL
    {
        #region Create part of C.R.U.D
        void AddBaseStation(BaseStation station);
        void AddDrone(Drone drone , int stationId);
        void AddParcel(Parcel parcel);
        void AddCustomer(Customer customer);
        #endregion

        #region Update part of C.R.U.d
        void ChargeDrone(int id);
        void DischargeDrone(int id, int time);
        void UpdateDrone(int id, string model);
        void UpdateBaseStation(int id, int count, string name);
        void UpdateCustomer(int id, string phone, string name);
        void LinkDroneToParcel(int id);
        #endregion

        #region Delete part of C.R.U.D

        #endregion

        #region Read part of C.R.U.D
        #region Read list of elements
        IEnumerable<DroneCharge> GetAllDroneCharges(int stationId);
        IEnumerable<BaseStation> GetAllBaseStations();
        IEnumerable<BaseStation> GetAllAvailableBAseStations();
        IEnumerable<Drone> GetAllDrones();
        IEnumerable<Customer> GetAllCustomers();
        IEnumerable<Parcel> GetAllParcels();
        IEnumerable<ParcelAtCustomer> GetAllOutGoingDeliveries(int senderId);
        IEnumerable<ParcelAtCustomer> GetAllIncomingDeliveries(int targetId);
        IEnumerable<Parcel> GetAllUnlinkedParcels();
        
        #endregion
        #region Read single element  
        BaseStation GetBaseStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);
        DroneInParcel GetDroneInParcel(int id);
        Location GetBasestationLocation(int id);
        ParcelInDelivery GetParcelInDelivery(int id);
        ParcelInList GetParcelInList(int id);
        #endregion
        #region Read Specific details of element
        int GetNearestBasestationID(Location l);
        int GetNearestParcelID(Location l, IEnumerable<Parcel> parcels);
        int GetParcelStatusIndicator(int id);
        CustomerInParcel GetCustomerInParcel(int senderId);
        bool CheckDistanceCoverageAbility(Drone dr,Parcel prc, WeightCategories w);
        
        #endregion
        #endregion







    }
}
