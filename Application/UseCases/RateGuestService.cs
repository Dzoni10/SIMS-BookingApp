using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.Application.UseCases
{
    public class RateGuestService
    {
        private readonly int limitedPeriod = 5;
        public AccommodationReservationService accommodationReservationService;
        public GuestService guestService;
        public AccommodationsService accommodationsService;
        public IGuestAccommodationRateRepository guestAccommodationRateRepository;
        
        public RateGuestService()
        {   
            accommodationReservationService = new AccommodationReservationService();
            accommodationsService = new AccommodationsService();
            guestService = new GuestService();
            guestAccommodationRateRepository = Injector.Injector.CreateInstance<IGuestAccommodationRateRepository>();
        }
        public void EliminateUnratedGuest()
        {

            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll())
            {
                if (DateTime.Now.Day - accommodationReservation.EndDate.Day > limitedPeriod && accommodationReservation.GuestRateStatus == RateStatus.Waiting)
                {
                    accommodationReservation.GuestRateStatus = RateStatus.NoRate;
                    accommodationReservationService.Update(accommodationReservation);
                }
            }
        }
        public List<AccommodationReservationDTO> FillReservations()
        {
            List<Guest> guests = guestService.GetAll();
            List<Accommodation> accommodations = accommodationsService.GetAccommodations();
            List<AccommodationReservationDTO> accommodationReservations = new List<AccommodationReservationDTO>();
            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll())
            {
                Guest correspondingGuest = guests.FirstOrDefault(guest => guest.Id == accommodationReservation.GuestId);
                Accommodation correspondingAccommodation = accommodations.FirstOrDefault(accommodation => accommodation.Id == accommodationReservation.AccommodationId);
                if (correspondingGuest != null && correspondingAccommodation != null && accommodationReservation.GuestRateStatus == RateStatus.Waiting)
                {
                    accommodationReservation.Guest = correspondingGuest;
                    accommodationReservation.Accommodation = correspondingAccommodation;
                    accommodationReservations.Add(new AccommodationReservationDTO(accommodationReservation));
                }
            }
            return accommodationReservations;
        }

       public void SaveGuestAccommodationRate(GuestAccommodationRate guestAccommodationRate)
        {
            guestAccommodationRateRepository.Save(guestAccommodationRate);
        }

        public void UpdateReservation(AccommodationReservation accommodationReservation)
        {
            accommodationReservationService.Update(accommodationReservation);
        }

        public bool AllowAttentionShow()
        {
            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll())
            {
                if (DateTime.Now.Day - accommodationReservation.EndDate.Day < limitedPeriod && accommodationReservation.GuestRateStatus == RateStatus.Waiting)
                {
                    return true;
                }
            }
            return false;
        }
    }
}