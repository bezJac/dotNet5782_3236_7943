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
        /// cunstructor for Add customer view of window 
        /// </summary>
        /// <param name="bL"> BL layer instance sent from previous window </param>
        public CustomerWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            addCustomerGrid.Visibility = Visibility.Visible;
            theBL = bl;
            newCustomer = new() { CustomerLocation = new(), To=null,From=null };
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

        /// <summary>
        /// update either name and/or phone number of customer
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
           
        }

        /// <summary>
        /// show details of parcel clicked on in list in Parcel Window
        /// </summary>
        private void ParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer prc;
            if (ParcelsFromListView.SelectedItem != null)
                prc = ParcelsFromListView.SelectedItem as ParcelAtCustomer;
            else
                prc = ParcelsToListView.SelectedItem as ParcelAtCustomer;
            if (prc != null)
            {
                new ParcelWindow(theBL, theBL.GetParcel(prc.Id)).Show();
                
            }
        }

        /// <summary>
        /// allows user to input numbers only to TextBox
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
        /// add customer inputed by user  to list 
        /// </summary>
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.AddCustomer(newCustomer);
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
                this.Activated -= refreshWindow;
                MessageBox.Show("Customer was added successfully to list", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                Closing += CloseWindowButton_Click;
                Close();
            }
        }

        /// <summary>
        /// cancel customer adding proccess
        /// </summary>
        private void CancelCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Closing += CloseWindowButton_Click;
            Close();
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
    }
}
