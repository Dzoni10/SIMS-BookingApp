﻿using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utilities;
using BookingApp.WPF.ViewModel;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for TouristTourRequestStatsWindow.xaml
    /// </summary>
    public partial class TouristTourRequestStatsWindow : Page
    {
        public TouristTourRequestStatsWindow(User user)
        {
            InitializeComponent();
            DataContext = new TouristTourRequestStatsWindowViewModel(this, user);
        }
    }
}
