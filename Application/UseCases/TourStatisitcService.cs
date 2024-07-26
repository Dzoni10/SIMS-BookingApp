using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace BookingApp.Application.UseCases
{
    public class TourStatisticsService
    {
        private TourReservationService tourReservationService;
        private TourGuestService tourGuestService;
        public TourStatisticsService(TourReservationService tourReservationService, TourGuestService tourGuestService)
        {
            this.tourReservationService = tourReservationService;
            this.tourGuestService = tourGuestService;
        }

        public List<TourGuest> GetTourGuests(int id)
        {;
            List<TourReservation> reservations = tourReservationService.GetByStartTourDateId(id);
            return reservations
            .SelectMany(reservation => tourGuestService.GetByReservationId(reservation.Id))
            .ToList();
        }

        public int CalculateUnder18(int id)
        {
            return GetTourGuests(id).Count(tourGuest => tourGuest.Age < 18);
        }

        public int CalculateBetween18_50(int id)
        {
            return GetTourGuests(id).Count(tourGuest => tourGuest.Age >= 18 && tourGuest.Age <= 50);
        }
        public int CalculateOver50(int id)
        {
            return GetTourGuests(id).Count(tourGuest => tourGuest.Age > 50);
        }
    }
}
