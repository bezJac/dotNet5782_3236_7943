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
        /// <summary>
        /// Add a new Base Station to list
        /// </summary>
        /// <param name="station"> Base Station to add </param>
        void AddBaseStation(BaseStation station);

        /// <summary>
        /// Add a new Drone to list
        /// </summary>
        /// <param name="drone"> drone to add</param>
        /// <param name="stationId"> station ID  of initial charge station for drone</param>
        void AddDrone(Drone drone,int StationId);

        /// <summary>
        /// Add a new Parcel to list
        /// </summary>
        /// <param name="parcel"> Parcel to add </param>
        void AddParcel(Parcel parcel);

        /// <summary>
        /// Add a new  Customer to list
        /// </summary>
        /// <param name="customer"> customer to add </param>
        void AddCustomer(Customer customer);
        #endregion

        #region Update part of C.R.U.D
        #region Update details of an entity
        /// <summary>
        /// Update an exsisting Base Station's name and/or total number of charging slots
        /// </summary>
        /// <param name="id"> ID of Base Station to update </param>
        /// <param name="count"> new count of  total charging slots </param>
        /// <param name="name"> new  name </param>
        void UpdateBaseStation(int id, int count, string name);

        /// <summary>
        /// Update an exsisting drone's model name 
        /// </summary>
        /// <param name="id"> ID of drone to update </param>
        /// <param name="model"> new model name </param>
        void UpdateDrone(int id, string model);

        /// <summary>
        /// update an exsisting customer's name or phone number
        /// </summary>
        /// <param name="id"> customer's ID</param>
        /// <param name="phone"> new phone number</param>
        /// <param name="name"> new name </param>
        void UpdateCustomer(int id, string phone, string name);
        #endregion
        #region Actions resulting in  updates to entities
        /// <summary>
        /// send a drone to a Base Station for charging
        /// </summary>
        /// <param name="id"> drone's ID </param>
        void ChargeDrone(int id);

        /// <summary>
        /// discharge a drone from current charging slot
        /// </summary>
        /// <param name="id"> drone's ID</param>
        /// <param name="time"> duration of charge in hours </param>
        void DischargeDrone(int id);

        /// <summary>
        /// Start delivery process by linking a drone to a compatible
        /// parcel to deliver
        /// </summary>
        /// <param name="id"> drone's ID</param>
        void LinkDroneToParcel(int id);

        /// <summary>
        ///  Pick up a parcel by its current linked drone
        /// </summary>
        /// <param name="id"> drone's ID </param>
        void DroneParcelPickUp(int id);

        /// <summary>
        /// deliver a parcel by its current linked drone
        /// </summary>
        /// <param name="id"> drone ID </param>
        void DroneParcelDelivery(int id);
        #endregion
        #endregion

        #region Read part of C.R.U.D
        #region Read list of elements
        /// <summary>
        /// Get copy list of all Base Stations in  BL BaseStation entities representation
        /// </summary>
        /// <returns> IEnumerable<BaseStation>  </returns>
        IEnumerable<BaseStation> GetAllBaseStations();

        /// <summary>
        /// Get copy list of all Base Stations in  BL BaseStationInList entities representation
        /// </summary>
        /// <returns> IEnumerable<BaseStationInList>  </returns>
        IEnumerable<BaseStationInList> GetALLBaseStationInList();

        /// <summary>
        /// get copy list of all Base Stations with available charging slots in  BL BaseStationInList entities representation
        /// </summary>
        /// <returns> IEnumerable<BaseStationInList>  </returns>
        IEnumerable<BaseStationInList> GetAllAvailablBaseStations();

        /// <summary>
        /// Get copy list of all drones in  BL Drone entities representation
        /// </summary>
        /// <returns> IEnumerable<Drone>  </returns>
        IEnumerable<Drone> GetAllDrones();

        /// <summary>
        /// get copy list of all drones in BL DroneInList representation
        /// </summary>
        /// <returns> IEnumerable<DroneInList>  </returns>
        IEnumerable<DroneInList> GetAllDronesInList(DroneStatus? status = null, WeightCategories? weight = null);

        /// <summary>
        ///  get copy list of all customers in  BL Customer entities representation
        /// </summary>
        /// <returns> IEnumerable<Customer>  </returns>
        IEnumerable<Customer> GetAllCustomers();

        /// <summary>
        /// get copy list of all customers  in  BL CustomerInList entities representation
        /// </summary>
        /// <returns> IEnumerable<CustomerInList> type </returns>
        IEnumerable<CustomerInList> GetAllCustomersInList();

        /// <summary>
        /// get copy list of all parcels in  BL Parcel entities representation
        /// </summary>
        /// <returns> IEnumerable<Parcel> type </returns>
        IEnumerable<Parcel> GetAllParcels();

        /// <summary>
        /// get copy list of all parcels in a BL ParcelInList representation
        /// </summary>
        /// <returns> IEnumerable<ParcelInList> type </returns>
        IEnumerable<ParcelInList> GetAllParcelsInList();

        /// <summary>
        /// get copy list of all unlinked to drone parcels, in  BL ParcelInList entities representation
        /// </summary>
        /// <returns> IEnumerable<ParcelInList>  </returns>
        IEnumerable<ParcelInList> GetAllUnlinkedParcels();

        /// <summary>
        /// get list of all outgoing parcels from a specific customer in BL ParcelAtCustomer entities representation
        /// </summary>
        /// <param name="senderId"> sending customer ID</param>
        /// <returns> IEnumerable<ParcelAtCustomer>  </returns>
        IEnumerable<ParcelAtCustomer> GetAllOutGoingDeliveries(int senderId);

        /// <summary>
        /// get list of all incoming  parcels to a specific customer in BL ParcelAtCustomer entities representation
        /// </summary>
        /// <param name="targetId"> target customer ID</param>
        /// <returns> IEnumerable<ParcelAtCustomer>  </returns>
        IEnumerable<ParcelAtCustomer> GetAllIncomingDeliveries(int targetId);

        /// <summary>
        /// get copy list of all drones charging in a specific Base Station in BL DroneCharge entities representation 
        /// </summary>
        /// <param name="stationId"> Base Station's ID</param>
        /// <returns>  IEnumerable<DroneCharge>  </returns>
        IEnumerable<DroneCharge> GetAllDronesCharging(int stationId);

        #endregion
        #region Read single element  
        /// <summary>
        /// get a base station by its ID
        /// </summary>
        /// <param name="id"> base stations's ID </param>
        /// <returns>  copy of BaseStation  matching the ID </returns>
        BaseStation GetBaseStation(int id);

        /// <summary>
        /// get a drone by its ID
        /// </summary>
        /// <param name="id"> drone's ID </param>
        /// <returns> copy of Drone matching the ID </returns>
        Drone GetDrone(int id);

        /// <summary>
        /// get a drone's  representation of DroneInParcel, by its ID
        /// </summary>
        /// <param name="id"> drone's ID </param>
        /// <returns> copy of drone matching the ID, in DroneInParcel representation </returns>
        DroneInParcel GetDroneInParcel(int id);

        /// <summary>
        /// get a customer by his ID
        /// </summary>
        /// <param name="id"> customer's ID </param>
        /// <returns> copy of customer matching the ID </returns>
        Customer GetCustomer(int id);

        /// <summary>
        /// get a parcel by its ID
        /// </summary>
        /// <param name="id"> parcel's ID </param>
        /// <returns> copy of parcel matching the ID </returns>
        Parcel GetParcel(int id);

        /// <summary>
        /// get  a customer's, customerInParcel representation,  by his ID 
        /// </summary>
        /// <param name="id"> customer's ID</param>
        /// <returns> copy of customer  matching the ID in  CustomerInParcel representation</returns>
        CustomerInParcel GetCustomerInParcel(int id);

        /// <summary>
        ///  get a parcel's ,  ParcelInDelivery representation, by its  ID 
        /// </summary>
        /// <param name="id"> parcel's ID </param>
        /// <returns> copy of parcel matching the ID in ParcelInDelivery representation </returns>
        ParcelInDelivery GetParcelInDelivery(int id);
        #endregion
        #endregion

    }
}
