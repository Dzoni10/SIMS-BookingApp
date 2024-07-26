using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using ceTe.DynamicPDF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModel
{
    public class ReservationAccommodationFormViewModel : INotifyPropertyChanged
    {
        private readonly User user;
        private ReservationAccommodationForm page;

        private AccommodationReservationService accommodationReservationService { get; set; }
        private AvailableReservationIntervalsService availableReservationIntervalsService { get; set; }
        private AccommodationsService accommodationsService { get; set; }
        private UserService userService { get; set; }
        private GuestService guestService { get; set; }

        private Accommodation selectedAccommodation { get; set; }
        public AccommodationReservationDTO AccommodationReservation { get; set; }
        public ObservableCollection<AccommodationReservationDTO> AvailableAccommodations { get; set; }
        public AccommodationReservationDTO SelectedReservation { get; set; }

        public RelayCommand DecreaseDaysReservedCommand { get; }
        public RelayCommand IncreaseDaysReservedCommand { get; }
        public RelayCommand DecreaseGuestCountCommand { get; }
        public RelayCommand IncreaseGuestCountCommand { get; }
        public RelayCommand SearchTimeSpansCommand { get; }
        public RelayCommand ConfirmReservationCommand { get; }


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

        public ReservationAccommodationFormViewModel(GuestWindow parentWindow, User user, ReservationAccommodationForm page, int accommodationId)
        {
            this.user = user;
            this.page = page;

            accommodationReservationService = new AccommodationReservationService();
            availableReservationIntervalsService = new AvailableReservationIntervalsService();
            accommodationsService = new AccommodationsService();
            userService = new UserService();
            guestService = new GuestService();
       
            AccommodationReservation = new AccommodationReservationDTO();
            selectedAccommodation = new Accommodation();
            AvailableAccommodations = new ObservableCollection<AccommodationReservationDTO>();

            AccommodationReservation.Accommodation = accommodationsService.GetById(accommodationId);
            selectedAccommodation = accommodationsService.GetById(accommodationId);
            AccommodationReservation.DaysReserved = selectedAccommodation.MinStayDays;
            AccommodationReservation.GuestCount = 1;
            AccommodationReservation.StartDate = DateTime.Now;
            AccommodationReservation.EndDate = DateTime.Now;

            DecreaseDaysReservedCommand = new RelayCommand(DecreaseDaysReservedExecute, CanExecute);
            IncreaseDaysReservedCommand = new RelayCommand(IncreaseDaysReservedExecute, CanExecute);
            DecreaseGuestCountCommand = new RelayCommand(DecreaseGuestCountExecute, CanExecute);
            IncreaseGuestCountCommand = new RelayCommand(IncreaseGuestCountExecute, CanExecute);
            SearchTimeSpansCommand = new RelayCommand(SearchTimeSpansExecute, CanExecute);
            ConfirmReservationCommand = new RelayCommand(ConfirmReservationExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }
        public void DecreaseDaysReservedExecute()
        {
            AccommodationReservation.DaysReserved = AccommodationReservation.DaysReserved > selectedAccommodation.MinStayDays ? AccommodationReservation.DaysReserved - 1 : AccommodationReservation.DaysReserved;
        }
        public void IncreaseDaysReservedExecute()
        {
            AccommodationReservation.DaysReserved++;
        }
        public void DecreaseGuestCountExecute()
        {
            AccommodationReservation.GuestCount = AccommodationReservation.GuestCount > 1 ? AccommodationReservation.GuestCount - 1 : AccommodationReservation.GuestCount;
        }
        public void IncreaseGuestCountExecute()
        {
            AccommodationReservation.GuestCount = AccommodationReservation.GuestCount < selectedAccommodation.Capacity ? AccommodationReservation.GuestCount + 1 : AccommodationReservation.GuestCount;
        }
        public void SearchTimeSpansExecute()
        {
            if (AccommodationReservation.EndDate.Subtract(AccommodationReservation.StartDate).Days < AccommodationReservation.DaysReserved)
            {
                ReservationFeedbackMessage = "The time span must be greater than the number of days for the reservation";
                page.ReservationFormFeedbackMessage.Foreground = Brushes.Red;
                return;
            }
            AvailableAccommodations.Clear();
            DisplayAvailableTimeSpans();

            if (availableReservationIntervalsService.GetAvailableTimeSpans(selectedAccommodation.Id, AccommodationReservation).Count == 0)
            {
                AccommodationReservation.EndDate = AccommodationReservation.EndDate.AddDays(10);
                availableReservationIntervalsService.GetAvailableTimeSpans(selectedAccommodation.Id, AccommodationReservation);
            }
        }
        public void ConfirmReservationExecute()
        {
            if (SelectedReservation == null)
            {
                ReservationFeedbackMessage = "First, you must select a possible TimeSpan";
                page.ReservationFormFeedbackMessage.Foreground = Brushes.Red;
                return;
            }
            //guestService.CheckSuperGuestStatus(userService.GetGuestIdFromUser(user));
            guestService.SuperGuestReservation(userService.GetGuestIdFromUser(user));
            CreateNewReservation();
            AvailableAccommodations.Clear();

            ReservationFeedbackMessage = "You have successfully booked the accommodation";
            page.ReservationFormFeedbackMessage.Foreground = Brushes.Green;
        }

        private void CreateNewReservation()
        {
            AccommodationReservation newReservation = new AccommodationReservation
            {
                AccommodationId = selectedAccommodation.Id,
                GuestId = userService.GetGuestIdFromUser(user),
                LocationId = selectedAccommodation.LocationId,
                StartDate = SelectedReservation.StartDate,
                EndDate = SelectedReservation.EndDate,
                GuestCount = AccommodationReservation.GuestCount,
                DaysReserved = AccommodationReservation.DaysReserved
            };

            accommodationReservationService.Save(newReservation);
        }
        public void DisplayAvailableTimeSpans()
        {
            var availableIntervals = availableReservationIntervalsService.GetAvailableReservationOptions(selectedAccommodation.Id, AccommodationReservation);
            availableIntervals.ToList().ForEach(interval => AvailableAccommodations.Add(interval));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
