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
        /// insrance of ListPresentor class to allow update of list in manager window from current window
        /// </summary>
        public static ListsPresentor listsPresentor { get; } = ListsPresentor.Instance;

        #region Constructors
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

            // set new parcel order choice possibilities
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
        #region Add Parcel grid methods
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
            catch (Exception ex) // add parcel faild- notify and allow user to fix input
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                TargetComboBox.BorderThickness = new Thickness(2);
                TargetComboBox.BorderBrush = Brushes.Red;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            if (flag)   // parcel was added successfully - notify and close window 
            {
                //this.Activated -= refresh;
                TargetComboBox.BorderThickness = new Thickness();
                MessageBox.Show("Parcel was added successfully to list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                listsPresentor.UpdateParcels();
                Closing += CloseWindowButton_Click;
                Close();
            }

        }
        /// <summary>
        /// cancel parcel add, and exit window
        /// </summary>
        private void CancelParcelButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            this.Close();
        }

        #endregion
        #region Actions on Parcel grid methods
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
            catch (Exception ex)   // remove faild - notify and stay in window
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (flag)     // remove was succesfull - notify and leave window ( no parcel to show)
            {
                // disable window refresh from activated event so return from message box to window before closing
                // wont throw exception on non exsisting parcel details trying to be reached
                win.Activated -= refreshWindow;
                MessageBox.Show("Parcel was removed successfully from list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                // update manager lists with change
                listsPresentor.UpdateParcels();
                // leave window
                Closing += CloseWindowButton_Click;
                Close();
            }
        }
        /// <summary>
        /// show details of drone or customers linked to parcel in a matching entity window
        /// generic function for all three buttons opens matching window by evaluating
        /// button name of routing button click
        /// </summary>
        private void fullDroneButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (fullDroneButton.Name == b.Name)
                new DroneWindow(theBL, theBL.GetDrone(newParcel.Drone.Id)).Show();

            if (fullSenderButton.Name == b.Name)
                new CustomerWindow(theBL, theBL.GetCustomer(newParcel.Sender.Id)).Show();

            if (fullTargetButton.Name == b.Name)
                new CustomerWindow(theBL, theBL.GetCustomer(newParcel.Sender.Id)).Show();
        }

        #endregion
        #region refresh and input validation methods
        /// <summary>
        /// refresh window content  - for action grid only
        /// </summary>
        private void refreshWindow(object sender, EventArgs e)
        {
            if (actionParcel.Visibility == Visibility.Visible)
            {
                newParcel = theBL.GetParcel(newParcel.Id);
                details.DataContext = newParcel;
                Detailsstk.DataContext = newParcel;
            }
        }
        #endregion
    }
}