using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// class manages observable collections for use in PL. (manager Window) 
    /// <para>each collection has methods to update entire collection or one entity </para>
    /// <para>class implements INotifyPropertyChanged Interface , PL lists Item Source is related by binding to collections, and content showing is changed 
    /// with any update to collections.</para>
    /// </summary>
    public class ListsPresentor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static IBL theBL = BlApi.BlFactory.GetBL();

        public static ListsPresentor Instance { get; } = new ListsPresentor();

        #region Drones Collection + update single entity/ entire  collection methods

        ObservableCollection<DroneInList> drones = new(theBL.GetAllDronesInList());

        public ObservableCollection<DroneInList> DronesList
        {
            get => drones;
            set
            {
                drones = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DronesList)));
            }
        }

        /// <summary>
        /// update collection to latest version of list in BL 
        /// <para> supports filtering by Max weight and/or Drone status</para>
        /// </summary>
        public void UpdateDronesView(DroneStatus? status =null,WeightCategories? weight =null )
        {
            try
            {
                DronesList = new(theBL.GetAllDronesInList(status, weight));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            } 
        }

        /// <summary>
        /// update drones collection with details of changes made to single Drone
        /// </summary>
        /// <param name="id"> drone's ID </param>
        public void UpdateSingleDrone(int id)
        {
            // get drone from collection
            DroneInList droneForList = DronesList.FirstOrDefault(d => d.Id == id);
            // get index of drone in collection
            int droneIndex = DronesList.IndexOf(droneForList);
            // if drone was found in collection - update its content with latest version returned from BL
            if (droneIndex >= 0)
            {
                DronesList.Remove(droneForList);
                DronesList.Insert(droneIndex, theBL.GetDroneFromList(id));
            }
        }
        #endregion

        #region Parcels Collection + update single entity/ entire collection methods

        ObservableCollection<ParcelInList> parcels = new(theBL.GetAllParcelsInList());

        public ObservableCollection<ParcelInList> ParcelsList
        {
            get => parcels;
            set
            {
                parcels = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ParcelsList)));
            }
        }

        /// <summary>
        /// update parcels collection to latest version of list in BL 
        /// <para> supports filtering by either max weight, parcel status, or priority </para>
        /// </summary>
        public void UpdateParcels(ParcelStatus? status = null, Priority? priority = null, WeightCategories? weight = null)
        {
            try
            {
                ParcelsList = new(theBL.GetAllParcelsInList(status, priority, weight));
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message,ex);
            }
            
        }

        /// <summary>
        /// update parcels collection to latest version of list in BL - 
        /// <para>  overloaded function to support updating list with filtering by date </para>
        /// </summary>
        public void UpdateParcelsByDate(DateTime? from, DateTime? to)
        {
            try
            {
                ParcelsList = new(theBL.GetAllParcelsInList(from, to));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }   
        }
        /// <summary>
        /// update collection with details of changes made to single parcel
        /// </summary>
        /// <param name="id"> parcel's ID </param>
        public void UpdateParcel(int id)
        {
            // get parcel from collection
            ParcelInList parcelForList = ParcelsList.FirstOrDefault(p => p.Id == id);
            // get index of parcel in collection
            int parcelIndex = ParcelsList.IndexOf(parcelForList);
            // if parcel was found in collection - update its content with lates version returned from BL
            if (parcelIndex >= 0)
            {
                ParcelsList.Remove(parcelForList);
                ParcelsList.Insert(parcelIndex, theBL.GetAllParcelsInList().Where(p => p.Id == parcelForList.Id).FirstOrDefault());
            }
        }
        #endregion

        #region Customers Collection + update single entity/ entire collection methods

        ObservableCollection<CustomerInList> customers = new(theBL.GetAllCustomersInList());

        public ObservableCollection<CustomerInList> CustomersList
        {
            get => customers;
            set
            {
                customers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomersList)));
            }
        }

        /// <summary>
        /// update customers collection to latest version of list in BL 
        /// </summary>
        public void UpdateCustomers()
        {
            CustomersList = new(theBL.GetAllCustomersInList());
        }
        /// <summary>
        /// update customers collection with details of changes made to a single customer
        /// </summary>
        /// <param name="id"> customer's ID </param>
        public void UpdateCustomer(int id)
        {
            // get customer from collection
            CustomerInList customerInList = CustomersList.FirstOrDefault(cs => cs.Id == id);
            // get index of customer in collection
            int CustomerIndex = CustomersList.IndexOf(customerInList);
            // if customer was found in collection - update its content with latest version returned from BL
            if (CustomerIndex >= 0)
            {
                CustomersList.Remove(customerInList);
                CustomersList.Insert(CustomerIndex, theBL.GetAllCustomersInList().Where(cs => cs.Id == id).FirstOrDefault());
            }
        }
        
        #endregion

        #region Stations Collection + update single entity/ entire list methods

        ObservableCollection<BaseStationInList> stations = new(theBL.GetALLBaseStationInList());

        public ObservableCollection<BaseStationInList> StationsList
        {
            get => stations;
            set
            {
                stations = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StationsList)));
            }
        }
        /// <summary>
        /// update stations collection to latest version of list in BL 
        /// </summary>
        public void UpdateStations()
        {
            StationsList = new(theBL.GetALLBaseStationInList());
        }
        /// <summary>
        /// update stations collection with details of changes made to a single station
        /// </summary>
        /// <param name="id"> station's ID </param>
        public void UpdateStation(int id)
        {
            // get station from collection
            BaseStationInList stationInList = StationsList.FirstOrDefault(st => st.Id == id);
            // get index of station in collection
            int stationIndex = StationsList.IndexOf(stationInList);
            // if station was found in collection - update its content with latest version returned from BL
            if (stationIndex >= 0)
            {
                StationsList.Remove(stationInList);
                StationsList.Insert(stationIndex, theBL.GetALLBaseStationInList().Where(st => st.Id == stationInList.Id).FirstOrDefault());
            }
        }
        #endregion
    }
}
