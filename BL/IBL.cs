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
        #endregion

        #region Delete part of C.R.U.D
        
        #endregion

        #region Read part of C.R.U.D
        #region Read list of elements
        IEnumerable<DroneCharge> GetAllDroneCharges();
        IEnumerable<BaseStation> GetAllBaseStations();
        IEnumerable<BaseStation> GetAllAvailableBAseStations();
        IEnumerable<Drone> GetAllDrones();
        IEnumerable<Customer> GetAllCustomers();
        IEnumerable<Parcel> GetAllParcels();
        IEnumerable<DeliveryAtCustomer> GetAllOutGoingDeliveries(int senderId);
        IEnumerable<DeliveryAtCustomer> GetAllIncomingDeliveries(int targetId);

        #endregion
        #region Read single element  
        BaseStation GetBaseStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);
        Delivery GetDelivery(int id);
        Location GetBasestationLocation(int id);
        #endregion
        #region Read Specific details of element
        int GetNearestBasestationID(Location l, IEnumerable<BaseStation> stations);
        int GetParcelStatusIndicator(int id);
        CustomerDelivery GetCustomerDetails(int senderId);
        
        #endregion
        #endregion







    }
}
