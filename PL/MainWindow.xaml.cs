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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BlApi.IBL myBL;
        private Customer newCustomer;
        /// <summary>
        /// cunstructor
        /// </summary>
        public MainWindow()
        {
            myBL = BlApi.BlFactory.GetBL();
            newCustomer = new Customer() { Id = null };
            InitializeComponent();
            IdCustomerTxt.DataContext = newCustomer;
            
            //this.txtBlck.Text = "HI !\nclick below\nto see the list of drones.";

        }

        /// <summary>
        /// Show Drones click - opens drone list window
        /// </summary>
        private void managerWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text == "admin" && adminPassword.Password == "123")
                new ManagerWindow(myBL).Show();
            else
                MessageBox.Show("username or password are incorrect", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            userName.Text = null;
            adminPassword.Password = null;
        }

        /// <summary>
        /// sign in for exsisting customer
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
            }
        }

        /// <summary>
        /// register as new customer
        /// </summary>
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
             new LoginWindow(myBL).Show();
        }

        private void exitProgram(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //foreach(var st in myBL.GetALLBaseStationInList())
            //{
            //    if(st.OccupiedSlots>0)
            //    {
                    
            //        foreach (DroneCharge dc in myBL.GetAllDronesCharging(st.Id) )
            //        {
            //            myBL.RemoveDroneCharge(dc.Id);
            //        }
            //    }
            //}
            App.Current.Shutdown();
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

        private void IdCustomerTxt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
