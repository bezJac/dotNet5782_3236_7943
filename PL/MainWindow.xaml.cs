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
        private IBL.IBL myBL;

        public MainWindow()
        {
            myBL = new BL.BL();
            InitializeComponent();

        }

        private void ShowDronesButton_Click(object sender, RoutedEventArgs e)
        {
            new ListDronesWindow(myBL).Show();
        }

     
    }
}