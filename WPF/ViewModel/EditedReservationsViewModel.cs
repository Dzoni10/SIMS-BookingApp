using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel
{
    public class EditedReservationsViewModel
    {
        public EditedReservations page;
        private readonly GuestWindow parentWindow;
        private readonly User user;

        private EditedReservationsService editedReservationsService;
        private AccommodationsService accommodationsService;
        private IgnoreService ignoreService;
        private ImageService imageService;
        public ObservableCollection<EditedReservationDTO> Reservations { get; set; }

        public EditedReservationsViewModel(GuestWindow parentWindow, User user, EditedReservations page)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;
            editedReservationsService = new EditedReservationsService();
            accommodationsService = new AccommodationsService();
            ignoreService = new IgnoreService();
            imageService = new ImageService();

            Reservations = new ObservableCollection<EditedReservationDTO>();

            LoadEditedReservations();
            HideNotificationIcon();
        }

        public void LoadEditedReservations()
        {
            foreach (EditedReservation editedReservation in editedReservationsService.GetEditedReservationsForUser(user))
            {
                List<Ignore> ignoredRequests = ignoreService.GetAll();
                Ignore ignoredRequest = ignoredRequests.FirstOrDefault(ignore => ignore.EditedReservationId == editedReservation.Id);
                EditedReservationDTO editedReservationDTO = new EditedReservationDTO(editedReservation);
                if(ignoredRequest != null)
                {
                    editedReservationDTO.OwnerComment = ignoredRequest.Explanation;
                }
                else
                {
                    editedReservationDTO.OwnerComment = " ";
                }
                editedReservationDTO.AccommodationName = accommodationsService.GetById(editedReservation.AccommodationId).Name;
                editedReservationDTO.Image = imageService.GetImageForAccommodation(editedReservation.AccommodationId);
                Reservations.Add(editedReservationDTO);
            }
        }
        public void HideNotificationIcon()
        {
            parentWindow.ExclamationIcon.Visibility = Visibility.Hidden;
            editedReservationsService.DeleteNotifications();
        }
    }
}
