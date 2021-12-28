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
    /// Interaction logic for userWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        /// <summary>
        /// instance of BL class object to access data for PL
        /// </summary>
        private readonly BlApi.IBL theBL;
        /// <summary>
        /// Customer object for window data context
        /// </summary>
        private Customer user;
        /// <summary>
        /// Parcel object for window data context
        /// </summary>
        private Parcel prc;
        /// <summary>
        /// cunstructor
        /// </summary>
        /// <param name="bl"> BL layer instance sent from previous window </param>
        /// <param name="cstmr"> Customer object containing data of customer sent from previous window</param>
        public UserWindow(BlApi.IBL bl,Customer cstmr)
        {
            InitializeComponent();
            theBL = bl;
            user = cstmr;
            prc = new();
            prc.Sender = theBL.GetCustomerInParcel((int)cstmr.Id);
            TargetComboBox.ItemsSource = from cst in theBL.GetAllCustomers()
                                         where cst.Id != cstmr.Id
                                         let c = new CustomerInParcel { Id= (int)cst.Id,Name=cst.Name}
                                         select c;
            parcelWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            priorityComboBox.ItemsSource= Enum.GetValues(typeof(Priority));
            DataContext = user;
            addParcel.DataContext = prc;
            ParcelsFromListView.ItemsSource = cstmr.From;
            ParcelsToListView.ItemsSource = cstmr.To;
        }
        /// <summary>
        /// add a new parcel delivery order for customer
        /// </summary>
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            theBL.AddParcel(prc);
            TargetComboBox.SelectedItem = null;
            parcelWeightComboBox.SelectedItem = null;
            priorityComboBox.SelectedItem=null;
            MessageBox.Show("Your order was placed successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
            user = theBL.GetCustomer((int)user.Id);
            DataContext = user;
            ParcelsFromListView.ItemsSource = user.From;
            ParcelsToListView.ItemsSource = user.To;
        }
        /// <summary>
        /// cancel new parcel delivery order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelParcelButton_Click(object sender, RoutedEventArgs e)
        {
            TargetComboBox.SelectedItem = null;
            parcelWeightComboBox.SelectedItem = null;
            priorityComboBox.SelectedItem = null;
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
    }

}
