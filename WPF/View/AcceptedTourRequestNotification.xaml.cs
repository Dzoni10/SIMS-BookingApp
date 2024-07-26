using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utilities;
using BookingApp.WPF.ViewModel;
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
    /// Interaction logic for AcceptedTourRequestNotification.xaml
    /// </summary>
    public partial class AcceptedTourRequestNotification : UserControl
    {
        public int NotificationId { get; set; }
        public AcceptedTourRequestNotification(int id, User user, int requestId, TouristNotificationWindowViewModel parentViewModel)
        {
            InitializeComponent();
            NotificationId = id;
            DataContext = new AcceptedTourRequestNotificationViewModel(this, user, requestId, parentViewModel);
        }
    }
}
