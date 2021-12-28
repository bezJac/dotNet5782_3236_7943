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
        private readonly IBL theBL;

        /// <summary>
        /// cunstructor
        /// </summary>
        /// <param name="bL"> BL layer instance sent from main window </param>
        public ManagerWindow(IBL bL)
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
        private void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        #region drones tab
        /// <summary>
        /// change in selected value of status comboBox of drones tab
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
                droneWindow.ChargeButton.Click += WindowSonButton_Click;
                droneWindow.DischargeButton.Click += WindowSonButton_Click;
                droneWindow.DeliverButton.Click += WindowSonButton_Click;
                droneWindow.PickUpButton.Click += WindowSonButton_Click;
                droneWindow.ScheduleButton.Click += WindowSonButton_Click;
                droneWindow.UpdateButton.Click += WindowSonButton_Click;


            }

        }
        /// <summary>
        /// clear selection in both comboBoxes in drones tab, list view shows all drones
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
                DroneListView.ItemsSource = theBL.GetAllDronesInList(null, (WeightCategories)WeightSelector.SelectedItem);

            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        /// <summary>
        /// group list view by selected checkBox parameter in drones tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RibbonCheckBox cb = sender as RibbonCheckBox;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
            PropertyGroupDescription groupDescription = null;

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
        /// <summary>
        /// cancel grouping view in drones tab
        /// </summary>
        private void DroneGroupChBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DroneListView.ItemsSource = theBL.GetAllDronesInList();
        }
        #endregion
        #region parcels tab
        /// <summary>
        /// change in selected value of status comboBox of parcels tab
        /// </summary>
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
        /// <summary>
        /// change in selected value of weight comboBox of parcels tab
        /// </summary>
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
        /// <summary>
        /// change in selected value of priority comboBox of parcels tab
        /// </summary>
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
        /// clear selection of comboBox's in parcels tab
        /// </summary>
        private void ParcelClearButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch (b.Name)
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
        /// double click on a parcel details in the list - 
        /// opens parcel window with ActionParcel grid showing
        /// </summary>
        private void ParcelList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListView.SelectedItem != null)
            {
                ParcelInList prc = ParcelListView.SelectedItem as ParcelInList;
                Parcel parcel = theBL.GetParcel(prc.Id);
                ParcelWindow parcelWindow = new ParcelWindow(theBL, parcel);
                parcelWindow.Show();
                parcelWindow.DummyButton.Click += WindowSonButton_Click;
                parcelWindow.removeParcelButton.Click += WindowSonButton_Click;
                ParcelListView.ItemsSource = theBL.GetAllParcelsInList(null, null, null);

            }

        }
        /// <summary>
        /// group list view by selected checkBox parameter in parcels tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelGroupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RibbonCheckBox cb = sender as RibbonCheckBox;
             List<ParcelInList> temp = new();
            switch (cb.Name)
            {
                case "SenderChbox":
                    {

                        var senderGrouping = from prc in theBL.GetAllParcelsInList()
                                             group prc by prc.SenderName into groups
                                             select groups;

                        foreach (var group in senderGrouping)
                            foreach (var item in group)
                                temp.Add(item);

                        ParcelListView.ItemsSource = temp;
                        break;
                    }
                case "TargetChbox":
                    {

                        var senderGrouping = from prc in theBL.GetAllParcelsInList()
                                             group prc by prc.TargetName into groups
                                             select groups;

                        foreach (var group in senderGrouping)
                            foreach (var item in group)
                                temp.Add(item);

                        ParcelListView.ItemsSource = temp;
                        break;



                    }
            }

        }
        /// <summary>
        /// cancel grouping view in drones tab
        /// </summary>
        private void ParcelGroupCheckBoxCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ParcelListView.ItemsSource = theBL.GetAllParcelsInList();
        }
        /// <summary>
        /// show in list parcels that were ordered between selected two dates
        /// </summary>
        private void toDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? from = fromDate.SelectedDate;
            DateTime? to = toDate.SelectedDate;
            if (to != null && from != null)
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
        #endregion
        #region customers tab
        /// <summary>
        /// double click on a customer details in the list - 
        /// opens customer window with ActionCustomer grid showing
        /// </summary>
        private void CustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListView.SelectedItem != null)
            {
                CustomerInList cs = CustomerListView.SelectedItem as CustomerInList;
                CustomerWindow win= new CustomerWindow(theBL, theBL.GetCustomer(cs.Id));
                win.UpdateButton.Click += WindowSonButton_Click;
                win.Show();           
            }
        }
        #endregion
        #region stations tab
        /// <summary>
        /// double click on a station details in the list - 
        /// opens station window with ActionStation grid showing
        /// </summary>
        private void StationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StationListView.SelectedItem != null)
            {
                BaseStationInList st = StationListView.SelectedItem as BaseStationInList;
                StationWindow win= new StationWindow(theBL, theBL.GetBaseStation(st.Id));
                win.UpdateButton.Click += WindowSonButton_Click;
                win.Show();
            }
        }
        /// <summary>
        /// group list view by selected checkBox parameter in stations tab
        /// </summary>
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
                        view.GroupDescriptions.Add(groupDescription);
                        break;
                    }
                case "AvailableSlotsGroupChBox":
                    {
                        List<BaseStationInList> temp = new();
                        var senderGrouping = from st in theBL.GetALLBaseStationInList()
                                             group st by st.AvailableSlots>0 into groups
                                             select groups;

                        foreach (var group in senderGrouping)
                            foreach (var item in group)
                                temp.Add(item);

                        StationListView.ItemsSource = temp;
                        break;                       
                    }
            }
        }
        /// <summary>
        /// cancel grouping view in stations tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationGroupChBox_Unchecked(object sender, RoutedEventArgs e)
        {
            StationListView.ItemsSource = theBL.GetALLBaseStationInList();
        }
        #endregion
        #region all tabs
        /// <summary>
        /// click on Add button - opens add window of object matching current selected tab
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch(b.Name)
            {
                case "addDroneButton":
                    {
                        DroneWindow win = new DroneWindow(theBL);
                        win.addButton.Click += WindowSonButton_Click;
                        win.Show();
                        break;
                        // refresh list view content in current window 
                    }
                case "addParcelButton":
                    {
                        ParcelWindow win = new ParcelWindow(theBL);
                        win.addButton.Click += WindowSonButton_Click;
                        win.Show();
                         break;
                    }
                case "addStationButton":
                    {
                        StationWindow win = new StationWindow(theBL);
                        win.addButton.Click += WindowSonButton_Click;
                        win.Show();
                        break;
                    }
                case "addCustomerButton":
                    {
                        CustomerWindow customerWindow = new CustomerWindow(theBL);
                        customerWindow.addButton.Click += WindowSonButton_Click;
                        customerWindow.Show();
                       
                        break;
                    }
            }
            

        }
        /// <summary>
        /// exit window
        /// </summary>
        private void CloseWindowButton_Click(object sender,System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            
        }
        /// <summary>
        /// current window reaction method (updates list views) to updates in son windows affecting lists
        /// </summary>
        private void WindowSonButton_Click(object sender, RoutedEventArgs e)
        {
            refreshListContent();
            ParcelListView.ItemsSource = theBL.GetAllParcelsInList();
            CustomerListView.ItemsSource = theBL.GetAllParcelsInList();
            StationListView.ItemsSource = theBL.GetALLBaseStationInList();
        }
        /// <summary>
        /// refresh list view content on tab selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab_Selected(object sender, RoutedEventArgs e)
        {
            TabItem tab = sender as TabItem;
            switch (tab.Header)
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
                        ParcelListView.ItemsSource = theBL.GetAllParcelsInList();
                        break;
                    }
                case "Stations":
                    {
                        StationListView.ItemsSource = theBL.GetALLBaseStationInList();
                        break;
                    }
            }

        }

        #endregion

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            Close();
        }
    }
}
