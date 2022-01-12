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
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        /// <summary>
        /// instance of BL class object to access data for PL
        /// </summary>
        private readonly BlApi.IBL theBL;

        /// <summary>
        /// BaseStation instance for data context of window
        /// </summary>
        private BaseStation newStation;

        /// <summary>
        /// insrance of ListPresentor class to allow update of list in manager window from current window
        /// </summary>
        public static ListsPresentor listsPresentor { get; } = ListsPresentor.Instance;

        /// <summary>
        /// by defualt both add and action grids Visibility is set to Collapsed
        /// constructur sets visibility of needed grid to Visible
        /// </summary>
        #region Constructors
        /// <summary>
        /// cunstructor for Add Base Station view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        public StationWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            theBL = bl;
            newStation = new() { StationLocation = new(), };
            DataContext = newStation;
            // make add grid visible 
            addStationGrid.Visibility = Visibility.Visible;

        }
        /// <summary>
        /// cunstructor for action base station view of window
        /// </summary>
        /// <param name="bl"> BL layer instance sent from previous window </param>
        /// <param name="station"> BaseStation object containing data of station sent from previous window</param>
        public StationWindow(BlApi.IBL bL, BaseStation station)
        {
            InitializeComponent();
            newStation = station;
            theBL = bL;
            DataContext = newStation;
            // make action grid visible
            actionStationGrid.Visibility = Visibility.Visible;
            DroneChargeListView.ItemsSource = newStation.DronesCharging;

        }
        #endregion

        #region Closing window execution methods
        /// <summary>
        /// exit window using close button only 
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            // add function to closing event to allow window close
            Closing += CloseWindowButton_Click;
            Close();
        }
        /// <summary>
        /// allow window close
        /// </summary>
        private void CloseWindowButton_Click(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = false;
        /// <summary>
        /// disables defualt X button from closing window
        /// </summary>
        private void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
        #endregion
        #region Add Station grid methods
        /// <summary>
        /// add base station with details from user input to the database
        /// </summary>
        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.AddBaseStation(newStation);
            }
            catch (Exception ex) // add station faild allow user to fix input
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            if (flag)   // drone was added successfully - close window 
            {
                MessageBox.Show("Station was added successfully to list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                // update manager list with new station
                listsPresentor.UpdateStations();
                // leave window
                Closing += CloseWindowButton_Click;
                Close();
            }
        }
        /// <summary>
        /// cancel station add, and exit window
        /// </summary>
        private void CancelStationButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            Close();
        }

        #endregion
        #region Actions on Station grid methods
        /// <summary>
        /// on double click on a drone in drones charging list open drone window for clicked drone
        /// </summary>
        private void DroneChargeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DroneChargeListView.SelectedItem != null)
            {
                // get drone charge entity to access drone's id
                DroneCharge dc = DroneChargeListView.SelectedItem as DroneCharge;
                // open drone window
                DroneWindow droneWindow = new DroneWindow(theBL, theBL.GetDrone(dc.Id));
                droneWindow.Show();
            }
        }

        /// <summary>
        /// update either name and/or number of charging slots of base station
        /// genertal method for both buttons - executs needed update by evaluting the button n ame routing the event
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch (b.Name)
            {
                case "RenameButton":
                    {
                        theBL.UpdateBaseStation((int)newStation.Id, newStation.DronesCharging.Count() + (int)newStation.NumOfSlots, newName.Text);
                        newName.Text = null;
                        break;
                    }
                case "UpdateButton":
                    {
                        theBL.UpdateBaseStation((int)newStation.Id, int.Parse(newChargeCount.Text), newStation.Name);
                        newChargeCount.Text = null;
                        break;
                    }
            }
            // refresh window to show update
            refreshWindow(sender, e);
            MessageBox.Show("Station details were updated", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
            // update manager list with new details
            listsPresentor.UpdateStation((int)newStation.Id);

        }

        #endregion
        #region refresh and input validation methods
        /// <summary>
        /// prevent user from typing non digit charachters to applied textBox
        /// </summary>
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }
        /// <summary>
        /// refresh window content - for action station grid
        /// </summary>
        private void refreshWindow(object sender, EventArgs e)
        {
            if (actionStationGrid.Visibility == Visibility.Visible)
            {
                newStation = theBL.GetBaseStation((int)newStation.Id);
                DroneChargeListView.ItemsSource = newStation.DronesCharging;
            }
        }
        #endregion
    }
}