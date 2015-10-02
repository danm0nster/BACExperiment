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

namespace BACExperiment.GUI
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        public PopUp(string message)
        {
            InitializeComponent();
            MessageLabel.Content = message;
        }

        private void PopUp_OnClosed(object sender, EventArgs e)
        {
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
