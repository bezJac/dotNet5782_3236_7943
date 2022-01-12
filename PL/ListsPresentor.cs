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
    public class ListsPresentor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static IBL theBL = BlApi.BlFactory.GetBL();
        public static ListsPresentor Instance { get; } = new ListsPresentor();


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
        public void UpdateDrones()
        {
            DronesList = new(theBL.GetAllDronesInList());
        }
        public void UpdateDrone(int id)
        {
            DroneInList droneForList = DronesList.FirstOrDefault(d => d.Id == id);
            int droneIndex = DronesList.IndexOf(droneForList);
            if (droneIndex >= 0)
            {
                DronesList.Remove(droneForList);
                DronesList.Insert(droneIndex, theBL.GetDroneFromList(id));
            }

        }



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
        public void UpdateParcels()
        {
            ParcelsList = new(theBL.GetAllParcelsInList());
        }
        public void UpdateParcel(int id)
        {
            ParcelInList parcelForList = ParcelsList.FirstOrDefault(p => p.Id == id);
            int parcelIndex = ParcelsList.IndexOf(parcelForList);
            if (parcelIndex >= 0)
            {
                ParcelsList.Remove(parcelForList);
                ParcelsList.Insert(parcelIndex, theBL.GetAllParcelsInList().Where(p => p.Id == parcelForList.Id).FirstOrDefault());
            }
        }




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
        public void UpdateCustomers()
        {
            CustomersList = new(theBL.GetAllCustomersInList());
        }
        public void UpdateStation(int id)
        {
            BaseStationInList stationInList = StationsList.FirstOrDefault(st => st.Id == id);
            int stationIndex = StationsList.IndexOf(stationInList);
            if (stationIndex >= 0)
            {
                StationsList.Remove(stationInList);
                StationsList.Insert(stationIndex, theBL.GetALLBaseStationInList().Where(st => st.Id == stationInList.Id).FirstOrDefault());
            }
        }



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
        public void UpdateStations()
        {
            StationsList = new(theBL.GetALLBaseStationInList());
        }
        public void UpdateCustomer(int id)
        {
            CustomerInList customerInList = CustomersList.FirstOrDefault(cs => cs.Id == id);
            int CustomerIndex = CustomersList.IndexOf(customerInList);
            if (CustomerIndex >= 0)
            {
                CustomersList.Remove(customerInList);
                CustomersList.Insert(CustomerIndex, theBL.GetAllCustomersInList().Where(cs => cs.Id == id).FirstOrDefault());
            }
        }



    }
}
