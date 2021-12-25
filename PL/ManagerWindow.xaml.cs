using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlApi;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for ListDronesWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        readonly private BlApi.IBL theBL;

        /// <summary>
        /// cunstructor
        /// </summary>
        /// <param name="bL"> BL layer instance sent from main window </param>
        public ManagerWindow(BlApi.IBL bL)
        {
            InitializeComponent();
            theBL = bL;
            // show all drones in drone list
            DroneListView.ItemsSource = theBL.GetAllDronesInList();
            ParcelListView.ItemsSource = theBL.GetAllParcelsInList();
            StationListView.ItemsSource = theBL.GetALLBaseStationInList();
            CustomerListView.ItemsSource = theBL.GetAllCustomersInList();
            // set values to comboBoxes
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            ParcelStatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            ParcelPrioritySelector.ItemsSource = Enum.GetValues(typeof(Priority));
            ParcelWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// change in selected value of status comboBox
        /// </summary>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null)
            {
                DroneStatus status = (DroneStatus)StatusSelector.SelectedItem;
                this.StatusSelector.Text = StatusSelector.SelectedItem.ToString();
                // filter list by new choice, and choice in weight comboBox if selected previously
                // exception accures when list returns empty
                try
                {
                    if(WeightSelector.SelectedItem != null)
                        DroneListView.ItemsSource = theBL.GetAllDronesInList(status, (WeightCategories)WeightSelector.SelectedItem);
                    else
                        DroneListView.ItemsSource = theBL.GetAllDronesInList(status);
                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    StatusSelector.SelectedItem = null;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ParcelStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParcelStatusSelector.SelectedItem != null)
            {
                ParcelStatus status = (ParcelStatus)ParcelStatusSelector.SelectedItem;
                this.ParcelStatusSelector.Text = ParcelStatusSelector.SelectedItem.ToString();
                // filter list by new choice, and choice in weight comboBox if selected previously
                // exception accures when list returns empty
                try
                {
                    
                    ParcelListView.ItemsSource = theBL.GetAllParcelsInList(status,null,null);
                    
                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    ParcelStatusSelector.SelectedItem = null;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ParcelWeightSelector.SelectedItem = null;
                    ParcelPrioritySelector.SelectedItem = null;
                }
            }
        }
        private void ParcelWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParcelWeightSelector.SelectedItem != null)
            {
                WeightCategories weight = (WeightCategories)ParcelWeightSelector.SelectedItem;
                this.ParcelWeightSelector.Text = ParcelWeightSelector.SelectedItem.ToString();
                // filter list by new choice, and choice in weight comboBox if selected previously
                // exception accures when list returns empty
                try
                {

                    ParcelListView.ItemsSource = theBL.GetAllParcelsInList(null, null, weight);

                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    ParcelWeightSelector.SelectedItem = null;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ParcelStatusSelector.SelectedItem = null;
                    ParcelPrioritySelector.SelectedItem = null;
                }
            }
        }
        private void ParcelPrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParcelPrioritySelector.SelectedItem != null)
            {
                Priority priority = (Priority)ParcelPrioritySelector.SelectedItem;
                this.ParcelPrioritySelector.Text = ParcelPrioritySelector.SelectedItem.ToString();
                // filter list by new choice, and choice in weight comboBox if selected previously
                // exception accures when list returns empty
                try
                {

                    ParcelListView.ItemsSource = theBL.GetAllParcelsInList(null, priority, null);

                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    ParcelWeightSelector.SelectedItem = null;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ParcelWeightSelector.SelectedItem = null;
                    ParcelStatusSelector.SelectedItem = null;
                }
            }
        }
        

        /// <summary>
        /// change in selected value of weight comboBox
        /// </summary>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem != null)
            {
                WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
                this.WeightSelector.Text = WeightSelector.SelectedItem.ToString();
                // filter list by new choice, and choice in status comboBox if selected previously
                // exception accures when list returns empty
                try
                {
                    if (StatusSelector.SelectedItem != null)
                        DroneListView.ItemsSource = theBL.GetAllDronesInList((DroneStatus)StatusSelector.SelectedItem, weight);
                    else
                        DroneListView.ItemsSource = theBL.GetAllDronesInList(null, weight);
                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    WeightSelector.SelectedItem = null;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                   
                }
            }
        }

        private void ParcelClearButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch(b.Name)
            {
                case "clearWeightButtonParcel":
                    {
                        ParcelWeightSelector.SelectedItem = null;
                        break;
                    }
                case "clearStatusButtonParcel":
                    {
                        ParcelStatusSelector.SelectedItem = null;
                        break;
                    }
                case "clearPriorityButtonParcel":
                    {
                        ParcelPrioritySelector.SelectedItem = null;
                        break;
                    }
                case "clearDatesButtonParcel":
                    {
                        
                        toDate.SelectedDate = null;
                        fromDate.SelectedDate = null;
                        break;
                    }
            }
            ParcelListView.ItemsSource = theBL.GetAllParcelsInList(null, null, null);

        }
        /// <summary>
        /// click on Add button - opens drone window with addDrone grid showing
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch(b.Name)
            {
                case "addDroneButton":
                    {
                        new DroneWindow(theBL).ShowDialog();
                        // refresh list view content in current window 
                        refreshListContent();
                        break;
                    }
                case "addParcelButton":
                    {
                        new ParcelWindow(theBL).ShowDialog();
                        ParcelListView.ItemsSource = theBL.GetAllParcelsInList(null, null, null);
                        break;
                    }
                case "addStationButton":
                    {
                        new StationWindow(theBL).ShowDialog();
                        StationListView.ItemsSource = theBL.GetALLBaseStationInList();
                        break;
                    }
                case "addCustomerButton":
                    {
                        CustomerWindow customerWindow = new CustomerWindow(theBL);
                        customerWindow.addButton.Click += AddButton_Click1;
                        customerWindow.Show();
                       
                        break;
                    }
            }
            

        }

        private void AddButton_Click1(object sender, RoutedEventArgs e)
        {
            CustomerListView.ItemsSource = theBL.GetAllCustomersInList();
            
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// double click on a drone details in the list - 
        /// opens drone window with ActionDrone grid showing
        /// </summary>
        private void DroneList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DroneListView.SelectedItem != null)
            {
                DroneInList dr = DroneListView.SelectedItem as DroneInList;
                Drone drone = theBL.GetDrone(dr.Id);
                DroneWindow droneWindow = new DroneWindow(theBL, drone);
                droneWindow.Show();
                droneWindow.ChargeButton.Click += DroneWindowSonButton_Click;
                droneWindow.DischargeButton.Click += DroneWindowSonButton_Click;
                droneWindow.DeliverButton.Click += DroneWindowSonButton_Click;
                droneWindow.PickUpButton.Click += DroneWindowSonButton_Click;
                droneWindow.ScheduleButton.Click += DroneWindowSonButton_Click;
                droneWindow.UpdateButton.Click += DroneWindowSonButton_Click;
                
               
            }

        }

        private void DroneWindowSonButton_Click(object sender, RoutedEventArgs e)
        {
            refreshListContent();
            ParcelListView.ItemsSource = theBL.GetAllParcelsInList();
            CustomerListView.ItemsSource = theBL.GetAllParcelsInList();
            StationListView.ItemsSource = theBL.GetALLBaseStationInList();
        }

        private void ParcelList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListView.SelectedItem != null)
            {
                ParcelInList prc = ParcelListView.SelectedItem as ParcelInList;
                Parcel parcel = theBL.GetParcel(prc.Id);
                ParcelWindow parcelWindow = new ParcelWindow(theBL, parcel);
                parcelWindow.Show();
                parcelWindow.DummyButton.Click += DroneWindowSonButton_Click;
                parcelWindow.removeParcelButton.Click += DroneWindowSonButton_Click;
                ParcelListView.ItemsSource = theBL.GetAllParcelsInList(null, null, null);

            }

        }
        private void StationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StationListView.SelectedItem != null)
            {
                BaseStationInList st = StationListView.SelectedItem as BaseStationInList;
                new StationWindow(theBL, theBL.GetBaseStation(st.Id)).ShowDialog();
                StationListView.ItemsSource = theBL.GetALLBaseStationInList();
            }
        }
        private void CustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(CustomerListView.SelectedItem!=null)
            {
                CustomerInList cs = CustomerListView.SelectedItem as CustomerInList;
                new CustomerWindow(theBL, theBL.GetCustomer(cs.Id)).ShowDialog();
                CustomerListView.ItemsSource = theBL.GetAllCustomersInList();
            }
        }
        /// <summary>
        /// clear selection in both comboBoxes, list view shows all drones
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusSelector.SelectedItem = null;
            this.WeightSelector.SelectedItem = null;
            DroneListView.ItemsSource = theBL.GetAllDronesInList();
            
        }

        /// <summary>
        /// refresh content of drone list view by current selections in ComboBoxes
        /// </summary>
        private void refreshListContent()
        {
            try
            {
                
                // nither comboBox has a selected choice - show all drones
                if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null) 
            {
                DroneListView.ItemsSource = theBL.GetAllDronesInList(null, null);
                return;
            }
            // both comboBoxes have selected choice - filter by both parameters
            if (WeightSelector.SelectedItem != null && StatusSelector.SelectedItem != null) 
            {
                DroneListView.ItemsSource = theBL.GetAllDronesInList((DroneStatus)StatusSelector.SelectedItem, (WeightCategories)WeightSelector.SelectedItem);
                return;
            }
            // only status comboBox has choice - filter by status
            if (StatusSelector.SelectedItem != null) 
            {
                DroneListView.ItemsSource = theBL.GetAllDronesInList((DroneStatus)StatusSelector.SelectedItem, null);
                return;
                
            }
            // only weight comboBox has choice - filter by weight
            DroneListView.ItemsSource = theBL.GetAllDronesInList( null, (WeightCategories)WeightSelector.SelectedItem);
            
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }
       

        private void Tab_Selected(object sender, RoutedEventArgs e)
        {
            TabItem tab = sender as TabItem;
            switch(tab.Header)
            {
                case "Drones":
                    {
                        refreshListContent();
                        break;
                    }
                case "Customers":
                    {
                        CustomerListView.ItemsSource = theBL.GetAllCustomersInList();
                        break;
                    }
                case "Parcels":
                    {
                        ParcelListView.ItemsSource= theBL.GetAllParcelsInList();
                        break;
                    }
                case "Stations":
                    {
                        StationListView.ItemsSource = theBL.GetALLBaseStationInList();
                        break;
                    }
            }
            
        }

        private void toDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? from = fromDate.SelectedDate;
            DateTime? to = toDate.SelectedDate;
            if (to != null && from!=null)
            {
                try
                {
                    ParcelListView.ItemsSource = theBL.GetAllParcelsInList(from, to);
                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.ClearButton_Click(sender, e);
                }
            }
        }

        private void ClearOutlinedComboBox_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            WeightSelector.SelectedItem = null;
            refreshListContent();
        }

        private void RibbonCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void GroupingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RibbonCheckBox cb = sender as RibbonCheckBox;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
            PropertyGroupDescription groupDescription =null;
           
            switch (cb.Name)
            {
                
                case "StatusGroupChBox":
                    {
                        groupDescription = new PropertyGroupDescription("Status");


                        break;
                    }
                case "WeightGroupChBox":
                    {
                       
                        groupDescription = new PropertyGroupDescription("MaxWeight");
                        break;
                    }
                case "ModelGroupChBox":
                    {
                        
                        groupDescription = new PropertyGroupDescription("Model");
                        
                        break;
                    }
                    
            }
            view.GroupDescriptions.Add(groupDescription);
        }

        private void DroneGroupChBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DroneListView.ItemsSource = theBL.GetAllDronesInList();
        }
        private void ParcelGroupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RibbonCheckBox cb = sender as RibbonCheckBox;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = null;
            switch (cb.Name)
            {
                case "SenderChbox":
                    {

                        groupDescription = new PropertyGroupDescription("Sender.Name");


                        break;
                    }
                    case "TargetChbox":
                    {

                        groupDescription = new PropertyGroupDescription("Target.Name");
                        

                        break;
                    }
            }
            view.GroupDescriptions.Add(groupDescription);
        }

        private void ParcelGroupCheckBoxCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ParcelListView.ItemsSource = theBL.GetAllParcelsInList();
        }

        private void StationGroupChBox_Unchecked(object sender, RoutedEventArgs e)
        {
            StationListView.ItemsSource = theBL.GetALLBaseStationInList();
        }

        private void StationGroupingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RibbonCheckBox cb = sender as RibbonCheckBox;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationListView.ItemsSource);
            PropertyGroupDescription groupDescription = null;
            switch (cb.Name)
            {
                case "NumSlotsGroupChBox":
                    {


                        groupDescription = new PropertyGroupDescription("AvailableSlots");

                        break;
                    }
                case "AvailableSlotsGroupChBox":
                    {

                        

                        break;
                    }
                   
            }
            view.GroupDescriptions.Add(groupDescription);

        }
    }
}
