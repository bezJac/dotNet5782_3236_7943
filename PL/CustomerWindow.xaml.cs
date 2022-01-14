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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        /// <summary>
        /// instance of BL class object to access data for PL
        /// </summary>
        private readonly BlApi.IBL theBL;

        /// <summary>
        /// Customer instance for data context of window
        /// </summary>
        private Customer newCustomer;

        /// <summary>
        /// insrance of ListPresentor class to allow update of list in manager window from current window
        /// </summary>
        public static ListsPresentor listsPresentor { get; } = ListsPresentor.Instance;

        #region Constructors
        /// <summary>
        /// cunstructor for Add customer view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        public CustomerWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            addCustomerGrid.Visibility = Visibility.Visible;
            theBL = bl;
            newCustomer = new() { CustomerLocation = new(), To = null, From = null };
            DataContext = newCustomer;
        }
        /// <summary>
        /// cunstructor for action customer view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        /// <param name="cstmr"> customer object containing details of customer sent from previos window </param>
        public CustomerWindow(BlApi.IBL bl, Customer cstmr)
        {
            InitializeComponent();
            actionCustomerGrid.Visibility = Visibility.Visible;
            newCustomer = cstmr;
            theBL = bl;
            DataContext = newCustomer;
            ParcelsFromListView.ItemsSource = newCustomer.From;
            ParcelsToListView.ItemsSource = newCustomer.To;
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
        #region Add Customer grid methods
        /// <summary>
        /// add customer inputed by user  to list 
        /// </summary>
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.AddCustomer(newCustomer);
            }
            catch (Exception ex) // add customer faild, notify and allow user to fix input
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                flag = false;
                MessageBox.Show(ex.Message, "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            if (flag)   // drone was added successfully - notify and close window 
            {
                MessageBox.Show("Customer was added successfully to list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                // update lists at manager window
                listsPresentor.UpdateCustomersView();
                Closing += CloseWindowButton_Click;
                Close();
            }
        }

        /// <summary>
        /// cancel customer adding proccess - close window
        /// </summary>
        private void CancelCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            Close();
        }

        #endregion
        #region Actions on Customer grid methods
        /// <summary>
        /// update either name and/or phone number of customer
        /// generic function for both buttons, determines updating choice by evaluating routing button's name
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch (b.Name)
            {
                case "RenameButton":
                    {
                        theBL.UpdateCustomer((int)newCustomer.Id, newCustomer.Phone, newNameTxtBox.Text);
                        newNameTxtBox.Text = null;
                        break;
                    }
                case "UpdateButton":
                    {
                        theBL.UpdateCustomer((int)newCustomer.Id, newPhoneTxtBox.Text, newCustomer.Name);
                        newPhoneTxtBox.Text = null;
                        break;
                    }
            }
            // update current window's content
            refreshWindow(sender, e);
            // update list in manager window
            listsPresentor.UpdateSingleCustomer((int)newCustomer.Id);

        }

        /// <summary>
        /// show details of parcel clicked on in list in Parcel Window
        /// </summary>
        private void ParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer prc = null;
            // check which list was clicked and get customer details
            if (ParcelsFromListView.SelectedItem != null)
                prc = ParcelsFromListView.SelectedItem as ParcelAtCustomer;
            else if (ParcelsToListView.SelectedItem != null)
                prc = ParcelsToListView.SelectedItem as ParcelAtCustomer;

            //open customer window
            new ParcelWindow(theBL, theBL.GetParcel(prc.Id)).Show();
        }


        #endregion
        #region refresh and input validation methods
        /// <summary>
        /// allows user to input numbers only to TextBox
        /// </summary>
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key is Key.Enter or Key.Return or Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key is Key.Escape or Key.Back or Key.Delete or
                Key.CapsLock or Key.LeftShift or Key.Home
             or Key.End or Key.Insert or Key.Down or Key.Right)
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
        /// refresh window content - for action customer grid
        /// </summary>
        private void refreshWindow(object sender, EventArgs e)
        {
            if (actionCustomerGrid.Visibility == Visibility.Visible)
            {
                newCustomer = theBL.GetCustomer((int)newCustomer.Id);
                DataContext = newCustomer;
                ParcelsFromListView.ItemsSource = newCustomer.From;
                ParcelsToListView.ItemsSource = newCustomer.To;
            }
        }
        #endregion
    }
}
