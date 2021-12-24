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
        BlApi.IBL theBL;
        BaseStation newStation; 
        public StationWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            theBL = bl;
            newStation = new() { StationLocation = new(), };
            DataContext = newStation;
            addStationGrid.Visibility = Visibility.Visible;
            LocationMessage.Text = "Important!!\nLattitude coordinae must be in 31.733 - 31.818 range!\nlongtitude coordinate must be in 35.167 - 35.243 range! ";

        }
        public StationWindow(BlApi.IBL bL, BaseStation station)
        {
            InitializeComponent();
            newStation = station;
            theBL = bL;
            DataContext = newStation;
            actionStationGrid.Visibility = Visibility.Visible;
            DroneChargeListView.ItemsSource = newStation.DronesCharging;
            RemoveStButton.DataContext = newStation;
        }

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

        private void RemoveStButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.RemoveBaseStation((int)newStation.Id);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (flag)
            {
                MessageBox.Show("Base Station was removed successfully from list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
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

        private void CancelStationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
