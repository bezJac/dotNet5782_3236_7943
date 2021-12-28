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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// instance of BL class object to access data for PL
        /// </summary>
        private readonly BlApi.IBL theBL;
        /// <summary>
        /// Customer object for window data context
        /// </summary>
        private Customer newUser;
        /// <summary>
        /// cunstructor
        /// </summary>
        /// <param name="bL"></param>
        public LoginWindow(BlApi.IBL bL)
        {
            theBL = bL;
            newUser = new() { CustomerLocation = new() };
            InitializeComponent();
            DataContext = newUser;
        }
        /// <summary>
        /// register new customer to data base
        /// </summary>
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                theBL.AddCustomer(newUser);
            }
            catch (Exception ex) // add drone faild allow user to fix input
            {
                flag = false;
                MessageBox.Show("Account already exists, please login with your ID", "INVALID", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
            }
            if (flag)   // drone was added successfully - close window 
            {
                MessageBox.Show("Your account was created successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
        /// <summary>
        /// cancel new customer registration
        /// </summary>
        private void CancelCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            Close();
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
