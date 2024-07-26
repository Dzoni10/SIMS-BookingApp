using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModel
{
    public class GuestViewModel : INotifyPropertyChanged
    {
        public GuestWindow page;
        private readonly User user;

        private GuestService guestService { get; set; }
        private UserService userService { get; set; }
        OwnerGuestNotificationRepository ownerGuestNotificationRepository { get; set; }
        public RelayCommand AccommodationsCommand { get; }
        public RelayCommand RateAccommodationCommand { get; }
        public RelayCommand AllReservationsCommand { get; }
        public RelayCommand ReservationRequestsCommand { get; }
        public RelayCommand GuestRatesCommand { get; }
        public RelayCommand GuestProfileCommand { get; }
        public RelayCommand AccommodationForumsCommand { get; }
        public RelayCommand AnywhereAnytimeCommand { get; }
        public RelayCommand BackCommand { get; }

        private bool exclamationIcon;
        public bool ExclamationIcon
        {
            get { return exclamationIcon; }
            set
            {
                if (value != exclamationIcon)
                {
                    exclamationIcon = value;
                    OnPropertyChanged("ExclamationIcon");
                }
            }
        }
        public GuestViewModel(User user, GuestWindow page)
        {
            this.user = user;
            this.page = page;

            guestService = new GuestService();
            userService = new UserService();

            ownerGuestNotificationRepository = new OwnerGuestNotificationRepository();
            ShowExclamationIcon();
            page.MainFrame.NavigationService.Navigate(new GuestAccommodations(page, user));
            guestService.CheckSuperGuestStatus(userService.GetGuestIdFromUser(user));

            AccommodationsCommand = new RelayCommand(AccommodationsExecute, CanExecute);
            RateAccommodationCommand = new RelayCommand(RateAccommodationExecute, CanExecute);
            AllReservationsCommand = new RelayCommand(AllReservationsExecute, CanExecute);
            ReservationRequestsCommand = new RelayCommand(ReservationRequestsExecute, CanExecute);
            GuestRatesCommand = new RelayCommand(GuestRatesExecute, CanExecute);
            GuestProfileCommand = new RelayCommand(GuestProfileExecute, CanExecute);
            AccommodationForumsCommand = new RelayCommand(AccommodationForumsExecute, CanExecute);
            AnywhereAnytimeCommand = new RelayCommand(AnywhereAnytimeExecute, CanExecute);
            BackCommand = new RelayCommand(BackExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }
        public void AccommodationsExecute()
        {
             page.MainFrame.NavigationService.Navigate(new GuestAccommodations(page, user));
        }
        public void RateAccommodationExecute()
        {
            page.MainFrame.NavigationService.Navigate(new RateAccommodations(page, user, ""));
        }
        public void AllReservationsExecute()
        {
            page.MainFrame.NavigationService.Navigate(new AccommodationReservations(page, user, ""));
        }
        public void ReservationRequestsExecute()
        {
            page.MainFrame.NavigationService.Navigate(new EditedReservations(page, user));
        }
        public void GuestRatesExecute()
        {
            page.MainFrame.NavigationService.Navigate(new GuestRates(page, user));
        }
        public void GuestProfileExecute()
        {
            page.MainFrame.NavigationService.Navigate(new GuestProfilePage(page, user));
        }
        public void AnywhereAnytimeExecute()
        {
            page.MainFrame.NavigationService.Navigate(new AnywhereAnytimePage(page, user));
        }
        public void AccommodationForumsExecute()
        {
            page.MainFrame.NavigationService.Navigate(new AccommodationForums(page, user));
        }
        private void ShowExclamationIcon()
        {
            if (ownerGuestNotificationRepository.GetAll().Count == 0)
            {
                //page.ExclamationIcon.Visibility = Visibility.Hidden;
                ExclamationIcon = false;
            }
            else
            {
                ExclamationIcon = true;
            }
        }
        public void BackExecute()
        {
            if (page.MainFrame.NavigationService.CanGoBack)
            {
                page.MainFrame.NavigationService.GoBack();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
