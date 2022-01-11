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
        private static  IBL theBL = BlApi.BlFactory.GetBL();
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
        public void updateDrones()
        {
            DronesList = new(theBL.GetAllDronesInList());
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


        
    }
}
