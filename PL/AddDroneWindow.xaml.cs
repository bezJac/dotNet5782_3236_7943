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
    public partial class AddDroneWindow : Window
    {
        private IBL.IBL theBL;
        private Drone newDrone;
        private int  stationID;
        public AddDroneWindow(IBL.IBL bL)
        {
            InitializeComponent();
            theBL = bL;
            newDrone = new Drone();
            this.maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        } 
        public AddDroneWindow(IBL.IBL bL,Drone exsistingDrone)
        {
            InitializeComponent();
            theBL = bL;
            newDrone = exsistingDrone;
            this.grid1.
            this.maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            bool flag = true;
            newDrone.Id = Convert.ToInt32(GetBindingExpression(TextBox.TextProperty));
            try
            {
                theBL.AddDrone(newDrone, stationID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
                flag = false;
            }
            if(flag)
            {
                MessageBox.Show("drone add successful", "", MessageBoxButton.OK);
                this.Close();
               
            }

        }

        

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
