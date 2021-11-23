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
        /// <summary>
        /// add a base station to stations list in data source layer
        /// </summary>
        /// <param name="st"> station object to be added </param>
        /// <exception cref = "ExsistException"> thrown if id already exists  </exception>
        void AddBaseStation(BaseStation st);

        /// <summary>
        /// add a drone to drones list in data source layer
        /// </summary>
        /// <param name="dr"> Drone object to be added </param>
        /// <exception cref = "ExsistException" > thrown if id already exists</exception>
        void AddDrone(Drone dr);

        /// <summary>
        /// add a customer to customers list in data source layer
        /// </summary>
        /// <param name="person"> customer object to be added </param>
        /// <exception cref = "ExsistException"> thrown if id already exists  </exception>
        void AddCustomer(Customer person);

        /// <summary>
        /// add a parcel to parcels list in data source layer
        /// </summary>
        /// <param name="pack"> parcel object to be added</param>
        /// <exception cref = "ExsistException"> thrown if id already exists   </exception>
        void AddParcel(Parcel pack);

        /// <summary>
        /// add a drone charge to  list in data source layer
        /// </summary>
        /// <param name="dc"> Drone chrge object to be added </param>
        /// <exception cref = "ExsistException" > thrown if id already exists</exception>
        void AddDroneCharge(DroneCharge dc);
        #endregion

        #region Update part of C.R.U.d
        /// <summary>
        /// update a base station in the list
        /// </summary>
        /// <param name="bst"> updated version of base station </param>
        ///  <exception cref = "NonExistsException"> thrown if id not found  </exception>
        void UpdateBaseStation(BaseStation bst);

        /// <summary>
        /// update a dronein the list
        /// </summary>
        /// <param name="dr"> updated version of drone </param>
        ///  <exception cref = "NonExistsException"> thrown if id not found </exception>
        void UpdateDrone(Drone dr);

        /// <summary>
        /// update a customer in the list
        /// </summary>
        /// <param name="person"> updated version of customer </param>
        ///  <exception cref = "NonExistsException"> thrown if id not found  </exception>
        void UpdateCustomer(Customer person);

        /// <summary>
        /// update a parcel in the list
        /// </summary>
        /// <param name="person"> updated version of parcel </param>
        ///  <exception cref = "NonExistsException"> thrown if id not founf  </exception>
        void UpdateParcel(Parcel pack);
        #endregion

        #region Delete part of C.R.U.D
        /// <summary>
        /// delete a base station from list
        /// </summary>
        /// <param name="bst"> base station to be  removed </param>
        ///  <exception cref = "NonExistsException"> thrown if id not found  </exception>
        void RemoveBaseStation(BaseStation bst);

        /// <summary>
        /// remove a drone from list
        /// </summary>
        /// <param name="dr"> drone to be  removed </param>
        /// <exception cref = "NonExistsException"> thrown if id not found  </exception>
        void RemoveDrone(Drone dr);

        /// <summary>
        /// remove a customer from list
        /// </summary>
        /// <param name="person"> customer to be removed </param>
        /// <exception cref = "NonExistsException"> thrown if id not found  </exception> 
        void RemoveCustomer(Customer person);

        /// <summary>
        /// remove a parcel from list
        /// </summary>
        /// <param name="pack"> parcel to be removed </param>
        /// <exception cref = "NonExistsException"> thrown if id not found  </exception> 
        void RemoveParcel(Parcel pack);

        /// <summary>
        /// remove a drone charge from list
        /// </summary>
        /// <param name="dc">  chrge to be removed </param>
        /// <exception cref = "NonExistsException"> thrown if id not found  </exception>
        void RemoveDroneCharge(DroneCharge dc);
        #endregion

        #region Read part of C.R.U.D
        #region Read list of elements
        ///  <returns> IEnumerable<BaseStation> type </returns>
        /// <summary>
        /// get a copy list of all base stations matching a predicate 
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<BaseStation> copy of full list , if predicate was sent as argument
        /// an IEnumerable<BaseStation> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        IEnumerable<BaseStation> GetAllBaseStations(Predicate<BaseStation> predicate = null);

        /// <summary>
        /// get a copy list  of drones 
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<Drone> copy of full list , if predicate was sent as argument
        /// an IEnumerable<Drone> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null);

        /// <summary>
        /// get a copy list containing of customers
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<Customer> copy of full list , if predicate was sent as argument
        /// an IEnumerable<Customer> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null);

        /// <summary>
        /// get a copy list cof parcels 
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<Parcel> copy of full list , if predicate was sent as argument
        /// an IEnumerable<Parcel> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null);

        /// <summary>
        /// get a copy list containing of drone charges  
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<Drone> copy of full list , if predicate was sent as argument
        /// an IEnumerable<Drone> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        IEnumerable<DroneCharge> GetAllDronecharges(Predicate<DroneCharge> predicate = null);
        #endregion
        #region Read single element  
        /// <summary>
        /// get a copy of a single base station 
        /// </summary>
        /// <param name = "id">  base station's ID </param>
        /// <exception cref = "NonExistsException"> thrown if id not founf in list </exception>
        /// <returns> copy of base station matching the id </returns>
        BaseStation GetBaseStation(int id);

        /// <summary>
        /// get a copy of a single Drone 
        /// </summary>
        /// <param name="id">  drone's ID </param>
        /// <exception cref="NonExistsException"> thrown if id not found in list </exception>
        /// <returns> copy of drone matching the id </returns>
        Drone GetDrone(int id);

        /// <summary>
        /// get a copy of a single customer 
        /// </summary>
        /// <param name="id">  customer's ID </param>
        /// <exception cref="NonExistsException"> thrown if id not found in list </exception>
        /// <returns> copy of customer matching the id </returns>
        Customer GetCustomer(int id);

        /// <summary>
        /// get a copy of a single parcal
        /// </summary>
        /// <param name="id">  parcel's ID </param>
        /// <exception cref="NonExistsException"> thrown if id not found in list </exception>
        /// <returns> copy of parcel matching the id </returns>
        Parcel GetParcel(int id);

        /// <summary>
        /// get a copy of a single drone charge entity
        /// </summary>
        /// <param name="droneId"> id of drone currently charging</param>
        /// <exception cref = "NonExistsException"> thrown if id not found </exception>
        /// <returns> copy of droneCharge entity matching the id </returns>
        DroneCharge GetDroneCharge(int droneId);
        #endregion
        #region Read specific details

        /// <summary>
        /// get array containing with DataSource.Config fields
        /// </summary>
        /// <returns> IEnumerable<double> </returns>
        IEnumerable<double> GetElectricUse();
        #endregion
        #endregion
    }
}
