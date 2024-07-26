using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModel
{
    public class AnywhereAnytimeViewModel
    {
        public AnywhereAnytimePage page;
        private readonly GuestWindow parentWindow;
        private readonly User user;

        private AccommodationSearchService accommodationSearchService;
        private ImageService imageService;
        private LocationService locationService;
        private UserService userService;
        private AvailableReservationIntervalsService availableReservationIntervalsService;

        public AccommodationReservationDTO AccommodationReservation { get; set; }
        public ObservableCollection<AccommodationReservationDTO> AvailableAccommodations { get; set; }
        public AccommodationReservationDTO SelectedReservation { get; set; }

        public RelayCommand DecreaseDaysReservedCommand { get; }
        public RelayCommand IncreaseDaysReservedCommand { get; }
        public RelayCommand DecreaseGuestCountCommand { get; }
        public RelayCommand IncreaseGuestCountCommand { get; }
        public RelayCommand SearchTimeSpansCommand { get; }
        public RelayCommand ViewAccommodationCommand { get; }
        public AnywhereAnytimeViewModel(GuestWindow parentWindow, User user, AnywhereAnytimePage page) {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            accommodationSearchService = new AccommodationSearchService();
            imageService = new ImageService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            userService = new UserService();
            availableReservationIntervalsService = new AvailableReservationIntervalsService();

            AccommodationReservation = new AccommodationReservationDTO();
            SelectedReservation = new AccommodationReservationDTO();
            AvailableAccommodations = new ObservableCollection<AccommodationReservationDTO>();

            AccommodationReservation.DaysReserved = 1;
            AccommodationReservation.GuestCount = 1;
            AccommodationReservation.StartDate = DateTime.Now;
            AccommodationReservation.EndDate = DateTime.Now;

            DecreaseDaysReservedCommand = new RelayCommand(DecreaseDaysReservedExecute, CanExecute);
            IncreaseDaysReservedCommand = new RelayCommand(IncreaseDaysReservedExecute, CanExecute);
            DecreaseGuestCountCommand = new RelayCommand(DecreaseGuestCountExecute, CanExecute);
            IncreaseGuestCountCommand = new RelayCommand(IncreaseGuestCountExecute, CanExecute);
            SearchTimeSpansCommand = new RelayCommand(SearchTimeSpansExecute, CanExecute);
            ViewAccommodationCommand = new RelayCommand(ViewAccommodationExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }
        public void DecreaseDaysReservedExecute()
        {
            AccommodationReservation.DaysReserved = AccommodationReservation.DaysReserved > 1 ? AccommodationReservation.DaysReserved - 1 : AccommodationReservation.DaysReserved;
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
            AccommodationReservation.GuestCount++;
        }
        public void SearchTimeSpansExecute()
        {
            // validacija datuma - start < end date

            if(AccommodationReservation.StartDate.Date != DateTime.Now.Date && AccommodationReservation.EndDate.Date != DateTime.Now.Date)
            {
                //search without dates
                if (AccommodationReservation.EndDate.Date.Subtract(AccommodationReservation.StartDate.Date).Days < AccommodationReservation.DaysReserved)
                {
                    //page.ErrorLabel.Content = "** Vremenski raspon mora biti veci od broja dana za rezervaciju";
                    //page.ErrorLabel.Foreground = Brushes.Red;
                    return;
                }
            }
            else
            {
                AccommodationReservation.StartDate = DateTime.Now;
                AccommodationReservation.EndDate = DateTime.Now.AddDays(60);
            }

            List<Accommodation> accommodations = accommodationSearchService.FilterAccommodationsByNumberOfGuests(AccommodationReservation.GuestCount);

            AvailableAccommodations.Clear();
            foreach (Accommodation accommodation in accommodations)
            {
                var availableIntervals = availableReservationIntervalsService.GetAvailableReservationOptions(accommodation.Id, AccommodationReservation);

                availableIntervals.ToList().ForEach(reservationDTO =>
                {
                    reservationDTO.Image = imageService.GetImageForAccommodation(accommodation.Id);
                    reservationDTO.Location = locationService.GetById(accommodation.LocationId);
                    reservationDTO.Type = accommodation.Type;

                    reservationDTO.GuestId = userService.GetGuestIdFromUser(user);
                    reservationDTO.AccommodationId = accommodation.Id;
                    reservationDTO.LocationId = accommodation.LocationId;
                    reservationDTO.GuestCount = AccommodationReservation.GuestCount;
                    reservationDTO.DaysReserved = AccommodationReservation.DaysReserved;
                    AvailableAccommodations.Add(reservationDTO);
                });
            }
        }
        public void ViewAccommodationExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new AccommodationPreviewPage(this.parentWindow, user, SelectedReservation.AccommodationId, SelectedReservation.ToAccommodationReservation(), "reserve"));
        }

    }
}
