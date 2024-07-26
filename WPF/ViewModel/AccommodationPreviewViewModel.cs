using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.DTO;
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

namespace BookingApp.WPF.ViewModel
{
    public class AccommodationPreviewViewModel : INotifyPropertyChanged
    {
        public AccommodationPreviewPage page;
        private readonly GuestWindow parentWindow;
        private readonly User user;

        public int accommodationId;
        private AccommodationsService accommodationService { get; set; }
        private ImageService imageService { get; set; }
        private OwnerAccommodationRateService ownerAccommodationRateService {get; set; }
        private AccommodationReservationService accommodationReservationService;

        public AccommodationDTO Accommodation { get; set; }
        public AccommodationReservation reservation { get; set; }

        public RelayCommand ReserveCommand { get; }
        public RelayCommand CreateReservationCommand { get; }

        private bool reserve;
        public bool Reserve
        {
            get { return reserve; }
            set
            {
                if (reserve != value)
                {
                    reserve = value;
                    OnPropertyChanged("Reserve");
                }
            }
        }

        public AccommodationPreviewViewModel(GuestWindow parentWindow, User user, int accommodationId, AccommodationReservation reservation, string viewMode)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            this.accommodationId = accommodationId;
            this.reservation = reservation;

            accommodationService = new AccommodationsService();
            imageService = new ImageService();
            ownerAccommodationRateService = new OwnerAccommodationRateService();
            accommodationReservationService = new AccommodationReservationService();

            Accommodation = new AccommodationDTO();

            LoadAccommodationData();

            ReserveCommand = new RelayCommand(ReserveExecute, CanExecute);
            CreateReservationCommand = new RelayCommand(CreateReservationExecute, CanExecute);

            if(viewMode == "reserve")
            {
                Reserve = true;
            }
            else
            {
                Reserve = false;
            }
        }
        public bool CanExecute()
        {
            return true;
        }
        public void ReserveExecute()
        {
            accommodationReservationService.Save(reservation);
            parentWindow.MainFrame.NavigationService.Navigate(new AccommodationReservations(this.parentWindow, user, "Reservation successfuly created"));
        }
        public void CreateReservationExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new ReservationAccommodationForm(this.parentWindow, user, accommodationId));
        }
        public void LoadAccommodationData()
        {
            Accommodation = new AccommodationDTO(accommodationService.GetById(accommodationId));
            Accommodation.Image = imageService.GetImageForAccommodation(accommodationId);
            double averageGrade = Math.Round(ownerAccommodationRateService.GetAverageGrade(accommodationId), 1);
            if(averageGrade > 0)
            {
                Accommodation.AverageRate = averageGrade.ToString();
            }
            else
            {
                Accommodation.AverageRate = "there is no grades";
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
