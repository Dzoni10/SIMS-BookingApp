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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel
{
    public class ReservationAccommodationEditFormViewModel
    {
        public ReservationAccommodationEditForm page;
        private GuestWindow parentWindow;
        private User user;

        private AccommodationsService accommodationsService { get; set; }
        private EditedReservationsService editedReservationsService { get; set; }
        private ImageService imageService { get; set; }

        public AccommodationReservationDTO AccommodationReservation { get; set; }
        public AccommodationReservationDTO SelectedReservation { get; set; }

        public RelayCommand ConfirmReservationCommand { get; }
        public ReservationAccommodationEditFormViewModel(GuestWindow parentWindow, User user, ReservationAccommodationEditForm page, AccommodationReservation reservation)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            accommodationsService = new AccommodationsService();
            editedReservationsService = new EditedReservationsService();
            imageService = new ImageService();

            AccommodationReservation = new AccommodationReservationDTO(reservation);
            AccommodationReservation.Accommodation = accommodationsService.GetById(reservation.AccommodationId);
            AccommodationReservation.Image = imageService.GetImageForAccommodation(reservation.AccommodationId);

            ConfirmReservationCommand = new RelayCommand(ConfirmReservationExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }
        public void ConfirmReservationExecute()
        {
            CreateEditedReservation();

            parentWindow.MainFrame.NavigationService.Navigate(new EditedReservations(parentWindow, user));
        }
        private void CreateEditedReservation()
        {
            EditedReservation editedReservation = new EditedReservation
            {
                AccommodationReservationId = AccommodationReservation.Id,
                AccommodationId = AccommodationReservation.Accommodation.Id,
                GuestId = AccommodationReservation.GuestId,
                StartDate = page.StartDatePicker.SelectedDate.Value,
                EndDate = page.StartDatePicker.SelectedDate.Value.AddDays(AccommodationReservation.DaysReserved),
                Status = ReservationStatus.Pending,
            };
            editedReservationsService.Save(editedReservation);
        }
    }
}
