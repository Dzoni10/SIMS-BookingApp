﻿using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for SelectCheckpointForm.xaml
    /// </summary>
    public partial class SelectCheckpointForm : Window
    {
        public SelectCheckpointForm()
        {
            InitializeComponent();
            this.DataContext = new SelectCheckpointViewModel();
        }
    }
}
