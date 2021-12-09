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
using IBL.BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBL.IBL myBL;

        /// <summary>
        /// cunstructor
        /// </summary>
        public MainWindow()
        {
            myBL = new BL.BL();
            InitializeComponent();
            txtBlck.Text = "HI !\nclick below\nto see the list of drones.";

        }

        /// <summary>
        /// Show Drones click - opens drone list window
        /// </summary>
        private void ShowDronesButton_Click(object sender, RoutedEventArgs e)
        {
            new ListDronesWindow(myBL).ShowDialog();
        }
    }
}
