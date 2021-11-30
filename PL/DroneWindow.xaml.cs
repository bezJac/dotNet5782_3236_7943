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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL theBL;
        private Drone newDrone;
        private BaseStationInList station;
        public DroneWindow(IBL.IBL bL)
        {

            InitializeComponent();
            theBL = bL;
            this.addDrone.Visibility = Visibility.Visible;
            this.actionDrone.Visibility = Visibility.Collapsed;
            newDrone = new Drone();
            DataContext = newDrone;
            this.maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            this.stationsList.ItemsSource = theBL.GetAllAvailablBaseStations();

           
        } 
        public DroneWindow(IBL.IBL bL,Drone exsistingDrone)
        {
            InitializeComponent();
            theBL = bL;
            newDrone = exsistingDrone;
            if (newDrone.Status == DroneStatus.Available||newDrone.Status==DroneStatus.Delivery)
                this.actionDrone.Children[6].Visibility = Visibility.Collapsed;
            this.maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            this.addDrone.Visibility = Visibility.Collapsed;
            this.actionDrone.Visibility = Visibility.Visible;
            this.droneView.Text = newDrone.ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                if (newDrone.Id <= 0)
                    throw new Exception("Id must be positive");
                station = stationsList.SelectedItem as BaseStationInList;
                theBL.AddDrone(newDrone,station.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
                flag = false;
            }
            if (flag)
            {
                MessageBox.Show("Drone was added successfully to list");
                
                this.Close();
                

            }
        }
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            newDrone.Model = newModel.Text.ToString();
            theBL.UpdateDrone(newDrone.Id, newDrone.Model);
            this.droneView.Text = newDrone.ToString();
        }

        private void ChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                theBL.ChargeDrone(newDrone.Id);
                MessageBox.Show("Drone charge in progress", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                this.droneView.Text = theBL.GetDrone(newDrone.Id).ToString();
                
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DischargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                theBL.DischargeDrone(newDrone.Id);
                MessageBox.Show("Drone charge ended", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                this.droneView.Text = theBL.GetDrone(newDrone.Id).ToString();
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                theBL.LinkDroneToParcel(newDrone.Id);
                MessageBox.Show("Drone is linked to a parcel", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                this.droneView.Text = theBL.GetDrone(newDrone.Id).ToString();
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PickUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                theBL.DroneParcelPickUp(newDrone.Id);
                MessageBox.Show("Drone picked up parcel", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                this.droneView.Text = theBL.GetDrone(newDrone.Id).ToString();
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                theBL.DroneParcelDelivery(newDrone.Id);
                MessageBox.Show("Parcel was delivered", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                this.droneView.Text = theBL.GetDrone(newDrone.Id).ToString();
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
