using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    public partial class ListDronesWindow : Window
    {
        readonly private BlApi.IBL theBL;

        /// <summary>
        /// cunstructor
        /// </summary>
        /// <param name="bL"> BL layer instance sent from main window </param>
        public ListDronesWindow(BlApi.IBL bL)
        {
            InitializeComponent();
            theBL = bL;
            // show all drones in drone list
            DroneListView.ItemsSource = theBL.GetAllDronesInList();
            // set values to comboBoxes
            this.StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            this.WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
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
                        this.DroneListView.ItemsSource = theBL.GetAllDronesInList(status, (WeightCategories)WeightSelector.SelectedItem);
                    else
                        this.DroneListView.ItemsSource = theBL.GetAllDronesInList(status);
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
                        this.DroneListView.ItemsSource = theBL.GetAllDronesInList((DroneStatus)StatusSelector.SelectedItem, weight);
                    else
                        this.DroneListView.ItemsSource = theBL.GetAllDronesInList(null, weight);
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
        /// click on Add button - opens drone window with addDrone grid showing
        /// </summary>
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(theBL).ShowDialog();
            // refresh list view content in current window 
            refreshListContent();

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
            DroneInList dr = DroneListView.SelectedItem as DroneInList;
            Drone drone = theBL.GetDrone(dr.Id);
            new DroneWindow(theBL, drone).ShowDialog();
            //after drone window close , refresh list content,
            // exception accures if list is empty after refresh
            try
            {
                refreshListContent();
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                this.ClearButton_Click(sender, e);
            }

        }

        /// <summary>
        /// clear selection in both comboBoxes, list view shows all drones
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusSelector.SelectedItem = null;
            this.WeightSelector.SelectedItem = null;
            this.DroneListView.ItemsSource = theBL.GetAllDronesInList();
        }

        /// <summary>
        /// refresh content of drone list view by current selections in ComboBoxes
        /// </summary>
        private void refreshListContent()
        {
            // nither comboBox has a selected choice - show all drones
            if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null) 
            {
                this.DroneListView.ItemsSource = theBL.GetAllDronesInList(null, null);
                return;
            }
            // both comboBoxes have selected choice - filter by both parameters
            if (WeightSelector.SelectedItem != null && StatusSelector.SelectedItem != null) 
            {
                this.DroneListView.ItemsSource = theBL.GetAllDronesInList((DroneStatus)StatusSelector.SelectedItem, (WeightCategories)WeightSelector.SelectedItem);
                return;
            }
            // only status comboBox has choice - filter by status
            if (StatusSelector.SelectedItem != null) 
            {
                this.DroneListView.ItemsSource = theBL.GetAllDronesInList((DroneStatus)StatusSelector.SelectedItem, null);
                return;
                
            }
            // only weight comboBox has choice - filter by weight
            this.DroneListView.ItemsSource = theBL.GetAllDronesInList( null, (WeightCategories)WeightSelector.SelectedItem);

        }
    }
}
