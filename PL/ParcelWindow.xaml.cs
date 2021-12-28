﻿using System;
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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        /// <summary>
        /// instance of BL class object to access data for PL
        /// </summary>
        private readonly BlApi.IBL theBL;
        /// <summary>
        /// parcel instance for data context of window
        /// </summary>
        private Parcel newParcel;

        /// <summary>
        /// cunstructor for Add Parcel view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        public ParcelWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            addParcel.Visibility = Visibility.Visible;
            theBL = bl;
            newParcel = new();
            DataContext = newParcel;
            parcelWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            IEnumerable<CustomerInParcel> tmp = from cstmr in theBL.GetAllCustomersInList()
                                                let cs = new CustomerInParcel { Id = cstmr.Id, Name = cstmr.Name }
                                                select cs;

            senderComboBox.ItemsSource = tmp;
            TargetComboBox.ItemsSource = tmp;
        }
        /// <summary>
        /// cunstructor for action parcel view of window
        /// </summary>
        /// <param name="bl"> BL layer instance sent from previous window </param>
        /// <param name="selectedParcel"> Parcel object containing data of parcel sent from previous window</param>
        public ParcelWindow(BlApi.IBL bl, Parcel selectedParcel)
        {

            InitializeComponent();
            actionParcel.Visibility = Visibility.Visible;
            theBL = bl;
            newParcel = selectedParcel;
            details.DataContext = newParcel;
            Detailsstk.DataContext = newParcel;
        }
        /// <summary>
        /// exit window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// add parcel with details from user input to the database
        /// </summary>
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.AddParcel(newParcel);
            }
            catch (Exception ex) // add drone faild allow user to fix input
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                TargetComboBox.BorderThickness = new Thickness(2);
                TargetComboBox.BorderBrush = Brushes.Red;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            if (flag)   // drone was added successfully - close window 
            {
                TargetComboBox.BorderThickness = new Thickness();
                MessageBox.Show("Parcel was added successfully to list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }

        }
        /// <summary>
        /// cancel parcel add, and exit window
        /// </summary>
        private void CancelParcelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// remove an unlinked parcel from database
        /// </summary>
        private void RemoveParcelButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.RemoveParcel(newParcel.Id);
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
                MessageBox.Show("Parcel was removed successfully from list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
        /// <summary>
        /// show details of drone linked to parcel in a drone window
        /// </summary>
        private void fullDroneButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (fullDroneButton.Name == b.Name)
            {
                DroneWindow droneWindow = new DroneWindow(theBL, theBL.GetDrone(newParcel.Drone.Id));
                droneWindow.Show();
                droneWindow.ChargeButton.Click += DummyButton_Click;
                droneWindow.DischargeButton.Click += DummyButton_Click;
                droneWindow.DeliverButton.Click += DummyButton_Click;
                droneWindow.PickUpButton.Click += DummyButton_Click;
                droneWindow.ScheduleButton.Click += DummyButton_Click;
                droneWindow.UpdateButton.Click += DummyButton_Click;
                newParcel = theBL.GetParcel(newParcel.Id);
                actionParcel.DataContext = newParcel;
                details.DataContext = newParcel;
                //return;
            }
            if (fullSenderButton.Name == b.Name)
            {
                new CustomerWindow(theBL, theBL.GetCustomer(newParcel.Sender.Id)).Show();
                //return;
            }
            if (fulltargetButton.Name == b.Name)
            {
                new CustomerWindow(theBL, theBL.GetCustomer(newParcel.Sender.Id)).Show();
                // return;
            }
        }

        private void DroneWindowSonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DummyButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
