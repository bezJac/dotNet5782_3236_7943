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
        public AddDroneWindow(IBL.IBL bL)
        {
            InitializeComponent();
            theBL = bL;
            newDrone = new Drone();
            this.MaxWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
    }
}
