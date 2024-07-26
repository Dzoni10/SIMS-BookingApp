﻿using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Image = BookingApp.Domain.Models.Image;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for TourCard.xaml
    /// </summary>
    public partial class TourCard : UserControl
    {

        public static readonly DependencyProperty TourNameProperty =
            DependencyProperty.Register("TourName", typeof(string), typeof(TourCard), new PropertyMetadata(null));

        public string TourName
        {
            get { return (string)GetValue(TourNameProperty); }
            set { SetValue(TourNameProperty, value); }
        }

        public TourCard(User user, string name, ShowToursViewModel viewModel)
        {
            InitializeComponent();
            TourName = name;
            DataContext = new TourCardViewModel(this, user, viewModel);
        }

    }
}
