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
        readonly private IBL.IBL theBL;
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
            if (StatusSelector.SelectedItem != null)
            {
                DroneStatus status = (DroneStatus)StatusSelector.SelectedItem;
                this.StatusSelector.Text = StatusSelector.SelectedItem.ToString();
                try
                {
                    if(WeightSelector.SelectedItem==null)
                         this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.Status == status);
                    else
                        this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.Status == status&&dr.MaxWeight==(WeightCategories)WeightSelector.SelectedItem);
                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem != null)
            {
                WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
                this.WeightSelector.Text = WeightSelector.SelectedItem.ToString();
                try
                {
                    if(StatusSelector.SelectedItem==null)
                         this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.MaxWeight == weight);
                    else
                        this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.MaxWeight == weight && dr.Status== (DroneStatus)StatusSelector.SelectedItem);
                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(theBL).ShowDialog();
            refreshList();   
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
         
            this.Close();
        }


        private void DroneList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneInList dr = DroneListView.SelectedItem as DroneInList;
            Drone drone = theBL.GetDrone(dr.Id);
            new DroneWindow(theBL, drone).ShowDialog();
            try
            {
                refreshList();
            }
            catch (Exception Ex)
            {
                while (Ex.InnerException != null)
                    Ex = Ex.InnerException;
                this.DroneListView.ItemsSource = null;
                MessageBox.Show(Ex.Message, "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
                e.Cancel =false;
            
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusSelector.SelectedItem = null;
            this.WeightSelector.SelectedItem = null;
            this.DroneListView.ItemsSource = theBL.GetAllDronesInList();
        }
        private void refreshList()
        {
            if(StatusSelector.SelectedItem==null)
            {
                if (WeightSelector.SelectedItem == null)
                    this.DroneListView.ItemsSource = theBL.GetAllDronesInList();
                else
                  this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr=>dr.MaxWeight==(WeightCategories) WeightSelector.SelectedItem);

            }
            else
            {
                if (WeightSelector.SelectedItem == null)
                    this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.Status == (DroneStatus)StatusSelector.SelectedItem);
                else
                    this.DroneListView.ItemsSource = theBL.GetAllDronesInList(dr => dr.MaxWeight == (WeightCategories)WeightSelector.SelectedItem && dr.Status == (DroneStatus)StatusSelector.SelectedItem);
            }
        }
    }

        
}
