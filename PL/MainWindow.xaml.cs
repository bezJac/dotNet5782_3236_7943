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

        /// <summary>
        /// cunstructor
        /// </summary>
        public MainWindow()
        {
            myBL = BlApi.BlFactory.GetBL();

            InitializeComponent();
        }

        #region Methods
        /// <summary>
        /// exit program - shut down all active windows
        /// </summary>
        private void exitProgram(object sender, System.ComponentModel.CancelEventArgs e) => Application.Current.Shutdown();
        /// <summary>
        /// opens program entry window 
        /// generic function for user and manager and new user ,
        /// window opened is determined by evaluating routing button's name
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "Manager": { new LoginWindow(myBL).ShowDialog(); break; }
                case "Customer": { new LoginWindow(myBL, 1).ShowDialog(); break; }
                case "Register": { new RegisterWindow(myBL).ShowDialog(); break; }
            }
        }
        #endregion
    }
}
