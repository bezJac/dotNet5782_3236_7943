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
using IBL.BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for ListDronesWindow.xaml
    /// </summary>
    public partial class ListDronesWindow : Window
    {
        private IBL.IBL theBL;
        public ListDronesWindow(IBL.IBL bL)
        {
            InitializeComponent();
            theBL = bL;
            DroneListView.ItemsSource = theBL.GetAllDronesInList();
            this.StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            this.WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)StatusSelector.SelectedItem;
            this.StatusSelector.Text = StatusSelector.SelectedItem.ToString();
            this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.Status == status);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
            this.WeightSelector.Text = WeightSelector.SelectedItem.ToString();
            this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.MaxWeight == weight);
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new AddDroneWindow(theBL).ShowDialog();
  
    
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }

        
}
