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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private readonly BlApi.IBL theBL;
        private Drone newDrone;
        private BaseStationInList station;

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
            // show action drone grid only
            actionDrone.Visibility = Visibility.Visible;
            theBL = bL;
            newDrone = exsistingDrone;
            actionDrone.DataContext = newDrone;
            DroneShow.DataContext = newDrone;
            switch (newDrone.Status)
            {
                case DroneStatus.Available: //charge and schedule buttons enabled
                    {
                        ChargeButton.Visibility = Visibility.Visible;
                        ScheduleButton.Visibility = Visibility.Visible;
                        break;
                    }
                case DroneStatus.Maintenance: // dischrge button enabled
                    {
                        DischargeButton.Visibility = Visibility.Visible;
                        break;
                    }
                case DroneStatus.Delivery:
                    {
                        if (newDrone.Parcel.InTransit) // drone already picked up parcel, deliver button enabled
                        {
                            DeliverButton.Visibility = Visibility.Visible;
                            break;
                        }
                        else   // drone only scheduled , pick up button enabled 
                        {
                            PickUpButton.Visibility = Visibility.Visible;
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// cancel drone add, exit window without adding drone to list
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Add button click in add drone grid view - add drone with details from user in window to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                station = stationsList.SelectedItem as BaseStationInList;
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
                MessageBox.Show("Drone was added successfully to list" ,"SUCCESS",MessageBoxButton.OK,MessageBoxImage.Information);
                Close();
            }
        }

        /// <summary>
        /// exit window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            
            //droneView.Text = newDrone.ToString();
        }

        /// <summary>
        /// send drone to charge at nearest base station
        /// </summary>
        private void ChargeButton_Click(object sender, RoutedEventArgs e)
        {
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
                ChargeButton.Visibility = Visibility.Collapsed;
                DischargeButton.Visibility = Visibility.Visible;
                ScheduleButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// release a drone from charging
        /// </summary>
        private void DischargeButton_Click(object sender, RoutedEventArgs e)
        {
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
                ChargeButton.Visibility = Visibility.Visible;
                DischargeButton.Visibility = Visibility.Collapsed;
                ScheduleButton.Visibility = Visibility.Visible;
            }
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
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
                ScheduleButton.Visibility = Visibility.Collapsed;
                PickUpButton.Visibility = Visibility.Visible;
                ChargeButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// send drone to pick up linked parcel
        /// </summary>
        private void PickUpButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.DroneParcelPickUp((int)newDrone.Id);
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
            if (flag) // drone picked up parcel successfully
            {
                MessageBox.Show("Drone picked up parcel", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                DroneShow.DataContext = newDrone;
                PickUpButton.Visibility = Visibility.Collapsed;
                DeliverButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// deliver parcel picked up by drone to targer customer
        /// </summary>
        private void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
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
                DeliverButton.Visibility = Visibility.Collapsed;
                ChargeButton.Visibility = Visibility.Visible;
                ScheduleButton.Visibility = Visibility.Visible;
            }
        }

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

        private void newModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButton.IsEnabled = true;
        }

        private void newModel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty((string)newModel.Text))
                UpdateButton.IsEnabled = false;
        }
    }
}
