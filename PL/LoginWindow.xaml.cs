using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for EnterManager.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly BlApi.IBL myBL;
        private Customer newCustomer;
        public LoginWindow(BlApi.IBL bL)
        {
            InitializeComponent();
            this.Top = 280;
            myBL = bL;
            manager.Visibility = Visibility.Visible;
        }
        public LoginWindow(BlApi.IBL bL, int dummy)
        {
            InitializeComponent();
            this.Top = 375;
            newCustomer = new Customer() { Id = null };
            IdCustomerTxt.DataContext = newCustomer;
            myBL = bL;
            customer.Visibility = Visibility.Visible;
            
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                newCustomer = myBL.GetCustomer((int)newCustomer.Id);
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                flag = false;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (flag)
            {
                IdCustomerTxt.Text = null;
                new UserWindow(myBL, newCustomer).Show();
            }
        }
        private void enterManager_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text == "admin" && adminPassword.Password == "123")
                new ManagerWindow(myBL).Show();
            else
                MessageBox.Show("username or password are incorrect", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            userName.Text = null;
            adminPassword.Password = null;
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

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
