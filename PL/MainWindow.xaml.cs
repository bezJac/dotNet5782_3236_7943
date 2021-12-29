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
            newCustomer = new();
            InitializeComponent();
            IdCustomerTxt.DataContext = newCustomer;
            //this.txtBlck.Text = "HI !\nclick below\nto see the list of drones.";

        }

        /// <summary>
        /// Show Drones click - opens drone list window
        /// </summary>
        private void managerWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text == "admin" && adminPassword.Password == "admin123")
                new ManagerWindow(myBL).ShowDialog();
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

      
    }
}
