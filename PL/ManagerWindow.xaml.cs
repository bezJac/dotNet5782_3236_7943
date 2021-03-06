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
        /// <summary>
        /// instance of BL class object to access data for PL
        /// </summary>
        private readonly IBL theBL;

        /// <summary>
        /// insrance of ListPresentor class to allow update of list in manager window from current window
        /// </summary>
        public static ListsPresentor ListsPresentor { get; } = ListsPresentor.Instance;

        /// <summary>
        /// cunstructor
        /// </summary>
        /// <param name="bL"> BL layer instance sent from main window </param>
        public ManagerWindow(IBL bL)
        {
            InitializeComponent();

            theBL = bL;
           
            droneListGrid.DataContext = ListsPresentor;
            parcelListGrid.DataContext = ListsPresentor;
            customerListGrid.DataContext = ListsPresentor;
            stationListgrid.DataContext = ListsPresentor;
            // set values to comboBoxes
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            ParcelStatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            ParcelPrioritySelector.ItemsSource = Enum.GetValues(typeof(Priority));
            ParcelWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        #region Closing window execution methods
        private void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
        /// <summary>
        /// exit window
        /// </summary>
        private void CloseWindowButton_Click(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

        }
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            Close();
        }
        #endregion

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
                    if (WeightSelector.SelectedItem != null)
                        ListsPresentor.UpdateDronesView(status, (WeightCategories)WeightSelector.SelectedItem);
                    else
                        ListsPresentor.UpdateDronesView(status,null);
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
                        ListsPresentor.UpdateDronesView((DroneStatus)StatusSelector.SelectedItem, weight);
                    else
                        ListsPresentor.UpdateDronesView(null, weight);
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
            }

        }
        /// <summary>
        /// clear selection in both comboBoxes in drones tab, list view shows all drones
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusSelector.SelectedItem = null;
            this.WeightSelector.SelectedItem = null;
            ListsPresentor.UpdateDronesView();

        }
        /// <summary>
        /// refresh content of drone list view by current selections in ComboBoxes
        /// </summary>
        private void refreshDroneListViewContent()
        {
            try
            {

                // nither comboBox has a selected choice - show all drones
                if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null)
                {
                    ListsPresentor.UpdateDronesView();
                    return;
                }
                // both comboBoxes have selected choice - filter by both parameters
                if (WeightSelector.SelectedItem != null && StatusSelector.SelectedItem != null)
                {
                    ListsPresentor.UpdateDronesView((DroneStatus)StatusSelector.SelectedItem, (WeightCategories)WeightSelector.SelectedItem);
                    return;
                }
                // only status comboBox has choice - filter by status
                if (StatusSelector.SelectedItem != null)
                {
                    ListsPresentor.UpdateDronesView((DroneStatus)StatusSelector.SelectedItem, null);
                    return;

                }
                // only weight comboBox has choice - filter by weight
                ListsPresentor.UpdateDronesView(null, (WeightCategories)WeightSelector.SelectedItem);

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
            // show grouped list
            view.GroupDescriptions.Add(groupDescription);
        }
        /// <summary>
        /// cancel grouping view in drones tab
        /// </summary>
        private void DroneGroupChBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // reset view to default list
            ListsPresentor.UpdateDronesView();
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
                   ListsPresentor.UpdateParcelsView(status, null, null);
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
                    // set other comboboxes choices to null
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
                    ListsPresentor.UpdateParcelsView(null, null, weight);
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
                    // set other comboboxes choices to null
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

                    ListsPresentor.UpdateParcelsView(null, priority, null);

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
                    // set other comboboxes choices to null
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
            ListsPresentor.UpdateParcelsView();

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
            // create grouping by sender or target names then set observable collection to grouped lists
            switch (cb.Name)
            {
                case "SenderChbox":
                    {

                        var senderGrouping = from prc in ListsPresentor.ParcelsList
                                             group prc by prc.SenderName into groups
                                             select groups;

                        foreach (var group in senderGrouping)
                            foreach (var item in group)
                                temp.Add(item);

                        ListsPresentor.ParcelsList =new(temp);
                        break;
                    }
                case "TargetChbox":
                    {

                        var senderGrouping = from prc in ListsPresentor.ParcelsList
                                             group prc by prc.TargetName into groups
                                             select groups;

                        foreach (var group in senderGrouping)
                            foreach (var item in group)
                                temp.Add(item);

                        ListsPresentor.ParcelsList = new(temp);
                        break;

                    }
            }

        }
        /// <summary>
        /// cancel grouping view in drones tab
        /// </summary>
        private void ParcelGroupCheckBoxCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListsPresentor.UpdateParcelsView();
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
                    ListsPresentor.UpdateParcelsView(from, to);
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
                CustomerWindow win = new CustomerWindow(theBL, theBL.GetCustomer(cs.Id));
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
                StationWindow win = new StationWindow(theBL, theBL.GetBaseStation(st.Id));
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
                case "NumSlotsGroupChBox":  // group by number of slots
                    {


                        groupDescription = new PropertyGroupDescription("AvailableSlots");
                        view.GroupDescriptions.Add(groupDescription);
                        break;
                    }
                case "AvailableSlotsGroupChBox": //group by number of sots only stations with available slots
                    {
                        List<BaseStationInList> temp = new();
                        var senderGrouping = from st in ListsPresentor.StationsList
                                             group st by st.AvailableSlots > 0 into groups
                                             select groups;

                        foreach (var group in senderGrouping)
                            foreach (var item in group)
                                temp.Add(item);

                        ListsPresentor.StationsList = new(temp);
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
            ListsPresentor.UpdateStationsView();
        }
        #endregion
        #region all tabs
        /// <summary>
        /// click on Add button - opens add window of object matching current selected tab.
        /// Determines window by evaluating routing button's name
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch (b.Name)
            {
                case "addDroneButton":
                    {
                        DroneWindow win = new DroneWindow(theBL);
                        win.Show();
                        break;
                    }
                case "addParcelButton":
                    {
                        ParcelWindow win = new ParcelWindow(theBL);
                        win.Show();
                        break;
                    }
                case "addStationButton":
                    {
                        StationWindow win = new StationWindow(theBL);
                        win.Show();
                        break;
                    }
                case "addCustomerButton":
                    {
                        CustomerWindow customerWindow = new CustomerWindow(theBL);
                        customerWindow.Show();
                        break;
                    }
            }
        }
        #endregion  
    }
}
