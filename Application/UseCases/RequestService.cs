using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class RequestService
    {
        public List<EditedReservationDTO> editedReservations;
        public EditedReservationsService editedReservationsService;
        public AccommodationsService accommodationsService;
        public GuestService guestService;
        public AccommodationReservationService accommodationReservationService;
        public OwnerGuestNotificationService ownerGuestNotificationService;
        public RequestService()
        {
            editedReservations = new List<EditedReservationDTO>();
            editedReservationsService = new EditedReservationsService();
            accommodationsService = new AccommodationsService();
            guestService = new GuestService();
            accommodationReservationService = new AccommodationReservationService();
            ownerGuestNotificationService = new OwnerGuestNotificationService();
        }
        public void SaveNotification(OwnerGuestNotification ownerGuestNotification)
        {
            ownerGuestNotificationService.SaveNotification(ownerGuestNotification);
        }
        public void UpdateAccommodationReservation(AccommodationReservation accommodationReservation)
        {
            accommodationReservationService.Update(accommodationReservation);
        }
       public List<EditedReservation> GetAllEditedReservations()
        {
            return editedReservationsService.GetAll();
        }
        public Accommodation GetAccommodationById(int id)
        {
            return accommodationsService.GetById(id);
        }
        public Guest GetGuestById(int id)
        {
            return guestService.GetById(id);
        }
        public List<AccommodationReservation> GetAllAccommodationReservations()
        {
            return accommodationReservationService.GetAll();
        }
        public EditedReservation GetEditedReservationById(int id)
        {
            return editedReservationsService.GetById(id);
        }
        public AccommodationReservation GetAccommodationReservationById(int id)
        {
            return accommodationReservationService.GetById(id);
        }
        public void UpdateEditedReservation(EditedReservation editedReservation)
        {
            editedReservationsService.Update(editedReservation);
        }
        public bool AllowWarningShow(EditedReservationDTO SelectedEditedReservation)
        {
            if (SelectedEditedReservation != null)
            {
                foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll())
                {
                    if (accommodationReservation.Id != SelectedEditedReservation.AccommodationReservationId && accommodationReservation.AccommodationId == SelectedEditedReservation.AccommodationId)
                    {
                        if (IsReservationDateOverlap(accommodationReservation, SelectedEditedReservation))
                            return true;
                    }
                }
                return false;
            }

            return false;
        }
        private bool IsReservationDateOverlap(AccommodationReservation accommodationReservation, EditedReservationDTO SelectedEditedReservation)
        {
            return IsDateInRange(SelectedEditedReservation.StartDate, accommodationReservation.StartDate, accommodationReservation.EndDate) ||
                   IsDateInRange(SelectedEditedReservation.EndDate, accommodationReservation.StartDate, accommodationReservation.EndDate) ||
                   IsDateIntercept(SelectedEditedReservation.StartDate, SelectedEditedReservation.EndDate, accommodationReservation.StartDate, accommodationReservation.EndDate);
        }
        private bool IsDateInRange(DateTime date, DateTime startDate, DateTime endDate)
        {
            return date > startDate && date < endDate;
        }
        private bool IsDateIntercept(DateTime startEditdate, DateTime endEditDate, DateTime startDate, DateTime endDate)
        {
            return startEditdate < startDate && endEditDate > endDate;
        }
    }
}
