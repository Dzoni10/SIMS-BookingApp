using BookingApp.Domain.Models;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;

namespace BookingApp.WPF.ViewModel
{
    public class NewTourNotificationViewModel
    {
        public NewTourNotification parentWindow { get; set; }
        public User LoggedInUser { get; set; }
        public int TourDateId { get; set; }
        public int NotificationId { get; set; }
        private TouristNotificationWindowViewModel ParentViewModel { get; set; }
        public RelayCommand InfoCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public NewTourNotificationViewModel(NewTourNotification window, User user, TouristNotificationWindowViewModel parentViewModel)
        {
            parentWindow = window;
            ParentViewModel = parentViewModel;
            LoggedInUser = user;
            TourDateId = parentWindow.TourDateId;
            NotificationId = parentWindow.NotificationId;
            InfoCommand = new RelayCommand(InfoClick);
            DeleteCommand = new RelayCommand(DeleteClick);
        }

        public void InfoClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new ShowTours(LoggedInUser));
        }

        public void DeleteClick()
        {
            ParentViewModel.DeleteNewTourNotification(parentWindow);
        }
    }
}
