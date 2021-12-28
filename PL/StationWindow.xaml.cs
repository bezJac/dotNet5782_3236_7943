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
        /// cunstructor for Add Base Station view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        public StationWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            theBL = bl;
            newStation = new() { StationLocation = new(), };
            DataContext = newStation;
            addStationGrid.Visibility = Visibility.Visible;
            LocationMessage.Text = "Important!!\nLattitude coordinae must be in 31.733 - 31.818 range!\nlongtitude coordinate must be in 35.167 - 35.243 range! ";

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
            actionStationGrid.Visibility = Visibility.Visible;
            DroneChargeListView.ItemsSource = newStation.DronesCharging;

        }
        /// <summary>
        /// exit window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
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
            catch (Exception ex) // add drone faild allow user to fix input
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            if (flag)   // drone was added successfully - close window 
            {
                MessageBox.Show("Station was added successfully to list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
        /// <summary>
        /// cancel station add, and exit window
        /// </summary>
        private void CancelStationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// open details of drone from drone charging list in drone window
        /// </summary>
        private void DroneChargeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(DroneChargeListView.SelectedItem!=null)
            {
                DroneCharge dc = DroneChargeListView.SelectedItem as DroneCharge;
            
                new DroneWindow(theBL, theBL.GetDrone(dc.Id)).ShowDialog();
                newStation = theBL.GetBaseStation((int)newStation.Id);
                DroneChargeListView.ItemsSource = newStation.DronesCharging;
                
            }
        }
        /// <summary>
        /// update either name and/or number of charging slots of base station
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch(b.Name)
            {
                case "RenameButton":
                    {
                        theBL.UpdateBaseStation((int)newStation.Id, newStation.DronesCharging.Count()+(int)newStation.NumOfSlots, newName.Text);
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
            newStation = theBL.GetBaseStation((int)newStation.Id);
            DataContext=newStation;
            MessageBox.Show("Station details were updated", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                
            
        }
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
    }
}
