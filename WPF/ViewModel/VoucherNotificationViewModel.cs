using BookingApp.Domain.Models;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel
{

    public class VoucherNotificationViewModel
    {
        public VoucherNotification parentWindow { get; set; }
        public int NotificationId;
        public User LoggedInUser { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        private TouristNotificationWindowViewModel ParentViewModel;

        public VoucherNotificationViewModel(VoucherNotification voucherNotification, User user, TouristNotificationWindowViewModel parentViewModel)
        {
            parentWindow = voucherNotification;
            NotificationId = parentWindow.NotificationId;
            LoggedInUser = user;
            ParentViewModel = parentViewModel;
            DeleteCommand = new RelayCommand(DeleteClick);

        }

        private void DeleteClick()
        {
            ParentViewModel.DeleteVoucherNotification(parentWindow);
        }
    }
}
