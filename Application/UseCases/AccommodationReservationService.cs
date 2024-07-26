using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class AccommodationReservationService
    {
        private IAccommodationReservationRepository accommodationReservationRepository;
        private AccommodationsService accommodationsService;
        private UserService userService;
        private ImageService imageService;
        private OwnerAccommodationRateService ownerAccommodationRateService { get; set; }
        public AccommodationReservationService()
        {
            accommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            accommodationsService = new AccommodationsService();
            userService = new UserService();
            imageService = new ImageService();
            ownerAccommodationRateService = new OwnerAccommodationRateService();
        }
        public bool isExist(int id)
        {
            foreach(AccommodationReservation accommodationReservation in accommodationReservationRepository.GetAll())
            {
                if (accommodationReservation.AccommodationId == id)
                    return true;
            }
            return false;
        }
        public AccommodationReservation GetById(int id)
        {
            return accommodationReservationRepository.GetById(id);
        }
        public List<AccommodationReservation> GetAll()
        {
            return accommodationReservationRepository.GetAll();
        }
        public void Delete(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Delete(reservation);
        }
        public void Save(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Save(reservation);
        }
        public void SaveAll(List<AccommodationReservation> accommodations)
        {
            accommodationReservationRepository.SaveAll(accommodations);
        }
        public void Update(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Update(reservation);
        }
        public bool IsReservationCancelingAvailable(DateTime startDate, int accommodationId)
        {
            TimeSpan daysLeft = startDate.Subtract(DateTime.Now);
            if (daysLeft.Days >= accommodationsService.GetById(accommodationId).CancelationPeriod)
                return true;
            return false;
        }
        public List<AccommodationReservation> GetReservationsForUser(User user)
        {
            int guestId = userService.GetGuestIdFromUser(user);
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in accommodationReservationRepository.GetAll())
            {
                if (reservation.GuestId == guestId && DateTime.Now.Date <= reservation.StartDate.Date)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }
        public void UpdateReservationRateStatus(int reservationId)
        {
            AccommodationReservation reservation = accommodationReservationRepository.GetById(reservationId);
            reservation.OwnerRateStatus = RateStatus.Rated;
            accommodationReservationRepository.Update(reservation);
        }
        public List<AccommodationReservationDTO> GetFinishedReservationsDTOsForGuest(int guestId)
        {
            var reservations = accommodationReservationRepository.GetAll().Where(r => r.GuestId == guestId);
            List<AccommodationReservationDTO> reservationDTOs = new List<AccommodationReservationDTO>();

            foreach (var reservation in reservations)
            {
                CalculateEndedRatingPeriod(reservation);

                if (DateTime.Now > reservation.EndDate)
                {
                    var reservationDTO = CreateReservationDTO(reservation);
                    reservationDTO.Image = imageService.GetImageForAccommodation(reservationDTO.AccommodationId);
                    reservationDTOs.Add(reservationDTO);
                }
            }
            return reservationDTOs;
        }
        public List<AccommodationReservation> GetFinishedReservationsForGuest(int guestId)
        {
            var allReservations = accommodationReservationRepository.GetAll().Where(r => r.GuestId == guestId);
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();

            foreach (var reservation in allReservations)
            {
                if (DateTime.Now > reservation.EndDate)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }
        public void CalculateEndedRatingPeriod(AccommodationReservation reservation)
        {
            if (DateTime.Now.Day > (reservation.EndDate.Day + 5) && reservation.OwnerRateStatus == RateStatus.Waiting)
            {
                reservation.OwnerRateStatus = RateStatus.NoRate;
                accommodationReservationRepository.Update(reservation);
            }
        }
        public AccommodationReservationDTO CreateReservationDTO(AccommodationReservation reservation)
        {
            AccommodationReservationDTO reservationDTO = new AccommodationReservationDTO(reservation)
            {
                RateDaysLeft = ownerAccommodationRateService.CalculateRateDaysLeft(reservation),
                Accommodation = accommodationsService.GetById(reservation.AccommodationId),
            };
            return reservationDTO;
        }
        public int CountReservationsInLastYear(int guestId)
        {
            List<AccommodationReservation> reservations = accommodationReservationRepository.GetAll().Where(r => r.GuestId == guestId).ToList();
            DateTime yearFromNow = DateTime.Now.AddMonths(-12);
            int reservationCount = 0;
            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.StartDate > yearFromNow && reservation.StartDate < DateTime.Now)
                {
                    reservationCount++;
                }
            }
            return reservationCount;
        }
        public Dictionary<int, int> ReservationsByMonths(int guestId)
        {
            Dictionary<int, int> monthReservationlMap = new Dictionary<int, int>();
            for (int i = 0; i < 12; i++)
            {
                monthReservationlMap.Add(DateTime.Now.AddMonths(i).Month, 0);
            }

            List<AccommodationReservation> reservations = accommodationReservationRepository.GetAll().Where(r => r.GuestId == guestId).ToList();
            foreach (AccommodationReservation reservation in reservations)
            {
                if(reservation.StartDate > DateTime.Now.AddYears(-1) && reservation.StartDate < DateTime.Now)
                {
                    monthReservationlMap[reservation.StartDate.Month]++;
                }
            }

            return monthReservationlMap;
        }

        public Dictionary<int, double> GetClosingAccommodation()
        {
            Dictionary<int, double> locationBusyness = new Dictionary<int, double>();
            
            foreach (AccommodationReservation acccommodationReservation in accommodationReservationRepository.GetAll())
            {
                if(!CheckKeyLocation(locationBusyness,acccommodationReservation.LocationId) )
                {
                        double busyness = CountAccommodationBusyness(acccommodationReservation.LocationId);
                        locationBusyness.Add(acccommodationReservation.LocationId, busyness);
                }
            }
            return locationBusyness;
        }

        public bool CheckKeyLocation(Dictionary<int,double> location,int locationId)
        {
            if (location.ContainsKey(locationId))
                return true;
            else
                return false;
        }
        public double CountAccommodationBusyness(int locationId)
        {
            double freeSpace = 0;
            double busyness = 0;

            foreach(AccommodationReservation accommodationReservation in accommodationReservationRepository.GetAll())
            {
                if(accommodationReservation.LocationId == locationId)
                {
                    busyness += accommodationReservation.GuestCount;
                    freeSpace +=(accommodationsService.GetAccommodationWithLocation(accommodationReservation.AccommodationId,locationId).Capacity) - busyness;
                }
            }
            if (freeSpace != 0)
                return busyness / freeSpace;
            else
                return -1.0;
        }

    }
}
