using BookingApp.Domain.Models;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for VoucherNotification.xaml
    /// </summary>
    public partial class VoucherNotification : UserControl
    {
        public int NotificationId { get; set; }
        public VoucherNotification(int id, User user, TouristNotificationWindowViewModel viewModel)
        {
            InitializeComponent();
            NotificationId = id;
            DataContext = new VoucherNotificationViewModel(this, user, viewModel);
        }

    }
}
