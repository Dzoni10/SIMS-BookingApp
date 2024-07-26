using BookingApp.Domain.Models;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using BookingApp.WPF.ViewModel;
using ceTe.DynamicPDF;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for NewTourNotification.xaml
    /// </summary>
    public partial class NewTourNotification : UserControl
    {
        public int TourDateId { get; set; }
        public int NotificationId { get; set; }
        public NewTourNotification(int id, User user, int tourDateId, TouristNotificationWindowViewModel viewModel)
        {
            InitializeComponent();
            TourDateId = tourDateId;
            NotificationId = id;
            DataContext = new NewTourNotificationViewModel(this, user, viewModel);
        }

    }
}
