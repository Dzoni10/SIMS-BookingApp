using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class ReviewService
    {
        public GuestService guestService;
        public OwnerAccommodationRateService ownerAccommodationRateService;
        public AccommodationsService accommodationsService;
        public AccommodationReservationService accommodationReservationService;
        public List<OwnerAccommodationRateDTO> OwnerAccommodationRates;
       
        public ReviewService()
        {
            guestService = new GuestService();
            ownerAccommodationRateService = new OwnerAccommodationRateService();
            accommodationsService = new AccommodationsService();
            accommodationReservationService = new AccommodationReservationService();
            OwnerAccommodationRates = new List<OwnerAccommodationRateDTO>();
        }
        public List<OwnerAccommodationRateDTO> GetAllOwnerAccommodationRates(){
            foreach (OwnerAccommodationRate ownerAccommodationRate in ownerAccommodationRateService.GetAll())
            {
                if (IsGuestRated(ownerAccommodationRate))
                {
                    ownerAccommodationRate.Guest = guestService.GetById(ownerAccommodationRate.GuestId);
                    ownerAccommodationRate.Accommodation = accommodationsService.GetById(ownerAccommodationRate.AccommodationId);
                    OwnerAccommodationRates.Add(new OwnerAccommodationRateDTO(ownerAccommodationRate));
                }
               
            }
            return OwnerAccommodationRates;
        }

        public bool IsGuestRated(OwnerAccommodationRate ownerAccommodationRate)
        {
            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll())
            {
                    if (accommodationReservation.GuestId == ownerAccommodationRate.GuestId && accommodationReservation.GuestRateStatus == RateStatus.Rated)
                        return true;
            }
            return false;
        }
    }
}
