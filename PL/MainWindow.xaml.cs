﻿using System;
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
            
            
            //this.txtBlck.Text = "HI !\nclick below\nto see the list of drones.";

        }

        /// <summary>
        /// Show Drones click - opens drone list window
        /// </summary>
       

        /// <summary>
        /// sign in for exsisting customer
        /// </summary>
        

        /// <summary>
        /// register as new customer
        /// </summary>
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
             new RegisterWindow(myBL).Show();
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

  
 

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "Manager": { new LoginWindow(myBL).Show(); break; }
                case "Customer": { new LoginWindow(myBL,1).Show(); break; }
                default:
                    break;
            }
            
        }
    }
}
