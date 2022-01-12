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

        #region Constructurs
        /// <summary>
        /// constructur for manager login - manager grid showing
        /// </summary>
        public LoginWindow(BlApi.IBL bL)
        {
            InitializeComponent();
            this.Top = 280;
            myBL = bL;
            manager.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// constructur for user login - customer grid showing
        /// </summary>
        public LoginWindow(BlApi.IBL bL, int dummy)
        {
            InitializeComponent();
            this.Top = 375;
            newCustomer = new Customer() { Id = null };
            IdCustomerTxt.DataContext = newCustomer;
            myBL = bL;
            customer.Visibility = Visibility.Visible;
        }
        #endregion
        #region Methods
        /// <summary>
        /// sign in for exsisting customer -  opens user window
        /// </summary>
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
                this.Close();
            }
        }

        /// <summary>
        /// log in as manager - opens manager window - password protected
        /// </summary>
        private void enterManager_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text == "admin" && adminPassword.Password == "123")
            {
                new ManagerWindow(myBL).Show();
                this.Close();
            }
            else
                MessageBox.Show("username or password are incorrect", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            userName.Text = null;
            adminPassword.Password = null;
            
        }

        /// <summary>
        /// allows user to input only digits to text box
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
        /// cancael entry to program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}