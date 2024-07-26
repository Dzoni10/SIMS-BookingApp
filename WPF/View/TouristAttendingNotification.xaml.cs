using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
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
    /// Interaction logic for TouristAttendingNotification.xaml
    /// </summary>
    public partial class TouristAttendingNotification : UserControl
    {
        public int NotificationId { get; set; }
        public TouristAttendingNotification(int id, User user, int startTourDateId, TouristNotificationWindowViewModel viewModel)
        {
            InitializeComponent();
            NotificationId = id;
            DataContext = new TouristAttendingNotificationViewModel(this, user, startTourDateId, viewModel);
        }
    }
}
