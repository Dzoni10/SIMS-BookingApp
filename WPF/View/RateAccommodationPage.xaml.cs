using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
using Microsoft.Win32;
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

namespace BookingApp.WPF.View
{
    public partial class RateAccommodationPage : Page
    {
        public RateAccommodationPage(GuestWindow parent, User user, int reservationId)
        {
            InitializeComponent();
            this.DataContext = new RateAccommodationViewModel(parent, user, this, reservationId);
            RenovationCommentTextBox.Text = "Add renovation reccommendation comment...";
            AdditionalCommentTextBox.Text = "Add additional comment...";
            RenovationCommentTextBox.Foreground = System.Windows.Media.Brushes.LightGray;
            AdditionalCommentTextBox.Foreground = System.Windows.Media.Brushes.LightGray;
        }

    }
}
