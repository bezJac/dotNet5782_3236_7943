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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL theBL;
        private Drone newDrone;
        public DroneWindow(IBL.IBL bL)
        {

            InitializeComponent();
            theBL = bL;
            newDrone = new Drone();
            DataContext = newDrone;
            this.maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
           
        } 
        public DroneWindow(IBL.IBL bL,Drone exsistingDrone)
        {
            InitializeComponent();
            theBL = bL;
            newDrone = exsistingDrone;
            this.maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            this.addDrone.Visibility = Visibility.Collapsed;
            this.actionDrone.Visibility = Visibility.Visible;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                if (newDrone.Id <= 0)
                    throw new Exception("Id must be positive");
                theBL.AddDrone(newDrone);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
                flag = false;
            }
            if (flag)
            {
                MessageBox.Show("Drone was added successfully to list");
                
                this.Close();
                

            }
        }

    }
}
