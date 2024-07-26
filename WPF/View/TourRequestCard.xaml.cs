using BookingApp.Domain.Models;
using BookingApp.DTO;
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
    /// <summary>
    /// Interaction logic for TourRequestCard.xaml
    /// </summary>
    public partial class TourRequestCard : UserControl
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<TourRequestGuestDTO> TourGuests { get; set; }
        public TourRequestCard(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            TourGuests = new ObservableCollection<TourRequestGuestDTO>();
        }
    }
}
