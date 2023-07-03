using SINGLE_STAGE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;


namespace SINGLE_STAGE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void MEVEButtonClicked(object sender, RoutedEventArgs e)
        {
            ManageEventsWindow MEW = new();
            MEW.Show();
            this.Close();
        }

        private void MARTButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void MPERButtonClicked(object sender, RoutedEventArgs e)
        {
            ManagePerformancesWindow MPW = new();
            MPW.Show();
            this.Close();
        }

        private void MAPPButtonClicked(object sender, RoutedEventArgs e)
        {
            ManageAppearancesWindow MAW = new();
            MAW.Show();
            this.Close();
        }

        private void MTKHButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void MTIXButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void CREPButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void EXITButtonClicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
