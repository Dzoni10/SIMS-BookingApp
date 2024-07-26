using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel
{
    public class RateAccommodationsViewModel : INotifyPropertyChanged
    {
        public RateAccommodations page;
        private readonly GuestWindow parentWindow;
        private readonly User user;

        private AccommodationReservationService accommodationReservationService { get; set; }
        private UserService userService { get; set; }
        public ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        public AccommodationReservationDTO SelectedReservation { get; set; }
        public RelayCommand RateAccommodationCommand { get; }
        public RelayCommand ViewRateCommand { get; }


        private string reservationFeedbackMessage;
        public string ReservationFeedbackMessage
        {
            get { return reservationFeedbackMessage; }
            set
            {
                if (value != reservationFeedbackMessage)
                {
                    reservationFeedbackMessage = value;
                    OnPropertyChanged("ReservationFeedbackMessage");
                }
            }
        }
        public RateAccommodationsViewModel(GuestWindow parentWindow, User user, RateAccommodations page, string feedbackMessage)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            accommodationReservationService = new AccommodationReservationService();
            userService = new UserService();
            Reservations = new ObservableCollection<AccommodationReservationDTO>();

            ReservationFeedbackMessage = feedbackMessage;

            RateAccommodationCommand = new RelayCommand(RateAccommodationExecute, CanExecute);
            ViewRateCommand = new RelayCommand(ViewRateExecute, CanExecute);
            ShowFinishedReservations();
        }
        public bool CanExecute()
        {
            return true;
        }
        public void RateAccommodationExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new RateAccommodationPage(parentWindow, user, SelectedReservation.Id));
        }
        public void ViewRateExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new RatedAccommodationPage(parentWindow, user, SelectedReservation.Id));
        }
        public void ShowFinishedReservations()
        {
            foreach(AccommodationReservationDTO reservationDTO in accommodationReservationService.GetFinishedReservationsDTOsForGuest(userService.GetGuestIdFromUser(user)))
            {
                if (reservationDTO.OwnerRateStatus != RateStatus.Waiting){
                    reservationDTO.RatedAccommodation = true;
                }
                else
                {
                    reservationDTO.RatedAccommodation = false;
                }
                Reservations.Add(reservationDTO);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
