﻿using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for RateTourAndGuide.xaml
    /// </summary>
    public partial class RateTourAndGuide : Page
    {
        public RateTourAndGuide(User user)
        {
            InitializeComponent();
            DataContext = new RateTourAndGuideViewModel(this, user);
        }

    }
}
