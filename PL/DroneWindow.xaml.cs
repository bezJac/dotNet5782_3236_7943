using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        /// <summary>
        /// instance of BL class object to access data for PL
        /// </summary>
        private readonly BlApi.IBL theBL;
        /// <summary>
        /// drone instance for data context of window
        /// </summary>
        private Drone newDrone;
        
        /// <summary>
        /// cunstructor for Add Drone view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        public DroneWindow(BlApi.IBL bL)
        {

            InitializeComponent();
            theBL = bL;
            // show add drone grid only
            addDrone.Visibility = Visibility.Visible;
            newDrone = new Drone();
            // set data context  for binding
            DataContext = newDrone;
            // set values of comboBoxes
            maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            stationsList.ItemsSource = theBL.GetAllAvailablBaseStations();
            
        }

        /// <summary>
        /// cunstructor for action drone view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        /// <param name="exsistingDrone"> drone containing details of drone sent from previos window </param>
        public DroneWindow(BlApi.IBL bL, Drone exsistingDrone)
        {
            InitializeComponent();
            actionDrone.Visibility = Visibility.Visible;
            theBL = bL;
            newDrone = exsistingDrone;
            actionDrone.DataContext = newDrone;
            DroneShow.DataContext = newDrone;
            Buttons.DataContext = newDrone;
        }

        /// <summary>
        /// cancel drone add, exit window without adding drone to list
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            Close();
        }

        /// <summary>
        /// Add button click in add drone grid view - add drone with details from user in window to the list
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                // check that input was made to all fields
                if(idAddTextBox.Text==string.Empty)
                {
                    idTextBox.BorderThickness = new Thickness(2);
                    idTextBox.BorderBrush = Brushes.Red;
                    throw new Exception("Drone's details missing");
                }
                if(modelTextBox.Text==string.Empty)
                {
                    modelTextBox.BorderThickness = new Thickness(2);
                    modelTextBox.BorderBrush = Brushes.Red;
                    throw new Exception("Drone's details missing");
                }
                if(maxWeightComboBox.SelectedItem==null|| stationsList.SelectedItem == null)
                {
                    throw new Exception("Drone's details missing");
                }
                
                if (newDrone.Id < 1000)
                {
                    idTextBox.BorderThickness = new Thickness(2);
                    idTextBox.BorderBrush = Brushes.Red;                   
                    throw new Exception("ID must be four digits");
                }

                // add drone to list
                BaseStationInList station = stationsList.SelectedItem as BaseStationInList;
                theBL.AddDrone(newDrone,station.Id);
                
            }
            catch (Exception ex) // add drone faild allow user to fix input
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK , MessageBoxImage.Warning);
                
            }
            if (flag)   // drone was added successfully - close window 
            {
                idTextBox.BorderThickness = new Thickness();
                this.Activated -= refreshWindow;
                MessageBox.Show("Drone was added successfully to list" ,"SUCCESS",MessageBoxButton.OK,MessageBoxImage.Information);
                Closing += CloseWindowButton_Click;
                Close();
            }
        }

        /// <summary>
        /// exit window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            Close();
        }
        private void CloseWindowButton_Click(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

        }
        private void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// update a drone's model 
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (newModel.Text != string.Empty)
            {
                theBL.UpdateDrone((int)newDrone.Id, newModel.Text);
                newDrone = theBL.GetDrone((int)newDrone.Id);
                DroneShow.DataContext = newDrone;
                MessageBox.Show("Model was updated", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                newModel.Text = null;
            }
            
          
        }

        /// <summary>
        /// send drone to charge at nearest base station
        /// </summary>
        private void ChargeButton_Click(object sender, RoutedEventArgs e)
        {
            actionDrone.DataContext = newDrone;
            bool flag = true;
            try
            {
                theBL.ChargeDrone((int)newDrone.Id);
                // get updated details of drone
                newDrone = theBL.GetDrone((int)newDrone.Id);
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                flag = false;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if(flag) // drone was sent to charge successfully 
            {
                MessageBox.Show("Drone charge in progress", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                DroneShow.DataContext = newDrone;
                Buttons.DataContext = newDrone;
                // ScheduleButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// release a drone from charging
        /// </summary>
        private void DischargeButton_Click(object sender, RoutedEventArgs e)
        {
            actionDrone.DataContext = newDrone;
            bool flag = true;
            try
            {
                theBL.DischargeDrone((int)newDrone.Id);
                // get updated details of drone
                newDrone = theBL.GetDrone((int)newDrone.Id);
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                flag = false;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if(flag)  // drone was releases from charge successfully 
            {
                MessageBox.Show("Drone charge ended", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                DroneShow.DataContext = newDrone;
                Buttons.DataContext = newDrone;
                // ScheduleButton.Visibility = Visibility.Visible;
            }
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            actionDrone.DataContext = newDrone;
            bool flag = true;
            try
            {
                theBL.LinkDroneToParcel((int)newDrone.Id);
                // get updated details of drone
                newDrone = theBL.GetDrone((int)newDrone.Id);
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                flag = false;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if(flag) // drone was linked successfully
            {
                MessageBox.Show("Drone is linked to a parcel", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                DroneShow.DataContext = newDrone;
                Buttons.DataContext = newDrone;
                //ScheduleButton.Visibility = Visibility.Collapsed;
                //PickUpButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// send drone to pick up linked parcel
        /// </summary>
        private void PickUpButton_Click(object sender, RoutedEventArgs e)
        {
            actionDrone.DataContext = newDrone;

            bool flag = true;
            try
            {
                theBL.DroneParcelPickUp((int)newDrone.Id);
                // get updated details of drone
                //newDrone = theBL.GetDrone((int)newDrone.Id);
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                flag = false;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (flag) // drone picked up parcel successfully
            {
                MessageBox.Show("Drone picked up parcel", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                refreshWindow(sender,e);
                
            }
        }

        /// <summary>
        /// deliver parcel picked up by drone to targer customer
        /// </summary>
        private void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            actionDrone.DataContext = newDrone;

            bool flag = true;
            try
            {
                theBL.DroneParcelDelivery((int)newDrone.Id);
                // get updated details of drone
                newDrone = theBL.GetDrone((int)newDrone.Id);
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                flag = false;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (flag) // drone delivered parcel successfully
            {
                
                MessageBox.Show("Parcel was delivered", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                DroneShow.DataContext = newDrone;
                Buttons.DataContext = newDrone;
            }
        }

        /// <summary>
        /// allows user to input numbers only to applied TextBox
        /// </summary>
        private void IdTextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
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
        /// check that input user for update for id is valid, notify with red border if not
        /// </summary>
        private void idTextValidation(object sender, RoutedEventArgs e)
        {
            
            if (idAddTextBox.Text.Length < 4)
            {
                idAddTextBox.BorderThickness = new Thickness(2);
                idAddTextBox.BorderBrush = Brushes.Red;
                
            }
            if(idAddTextBox.Text.Length == 4 )
            {
                idAddTextBox.BorderThickness = new Thickness(0);
            }
        }

        /// <summary>
        /// check that input user for update for model is valid, notify with red border if not
        /// </summary>
        private void modelTextValidation(object sender, RoutedEventArgs e)
        {
            if(modelTextBox.Text==string.Empty)
            {
                modelTextBox.BorderThickness = new Thickness(2);
                modelTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                modelTextBox.BorderThickness = new Thickness(0);
            }
        }

        private void refreshWindow(object sender, EventArgs e)
        {
            if (actionDrone.Visibility == Visibility.Visible)
            {
                newDrone = theBL.GetDrone((int)newDrone.Id);
                actionDrone.DataContext = newDrone;
                DroneShow.DataContext = newDrone;
                Buttons.DataContext = newDrone;
            }
        }
    }
}
