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
        BlApi.IBL theBL;
        Customer user;
        Parcel prc;
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

        private void CancelParcelButton_Click(object sender, RoutedEventArgs e)
        {
            TargetComboBox.SelectedItem = null;
            parcelWeightComboBox.SelectedItem = null;
            priorityComboBox.SelectedItem = null;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
