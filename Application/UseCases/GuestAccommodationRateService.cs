using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.View;
using iText.StyledXmlParser.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    class GuestAccommodationRateService
    {
        private IGuestAccommodationRateRepository guestAccommodationRateRepository;
        private OwnerAccommodationRateService ownerAccommodationRateService;
        private AccommodationReservationService accommodationReservationService;

        public GuestAccommodationRateService()
        {
            guestAccommodationRateRepository = Injector.Injector.CreateInstance<IGuestAccommodationRateRepository>();

            ownerAccommodationRateService = new OwnerAccommodationRateService();
            accommodationReservationService = new AccommodationReservationService();
        }
        public List<GuestAccommodationRate> GetAll()
        {
            return guestAccommodationRateRepository.GetAll();
        }
        public List<GuestAccommodationRate> GetFilteredRates(int guestId)
        {
            List<GuestAccommodationRate> filteredGuestRates = new List<GuestAccommodationRate>();
            AccommodationReservation reservation;
            foreach (GuestAccommodationRate guestRate in guestAccommodationRateRepository.GetAll())
            {
                if (guestRate.GuestId == guestId)
                {
                    reservation = accommodationReservationService.GetById(guestRate.AccommodationReservationId);
                    if (reservation.OwnerRateStatus == RateStatus.Rated && reservation.GuestRateStatus == RateStatus.Rated)
                    {
                        filteredGuestRates.Add(guestRate);
                    }
                }
            }

            return filteredGuestRates;
        }
        public double GetAverageCleanlinessRate(int guestId)
        {
            List<GuestAccommodationRate> guestRates = guestAccommodationRateRepository.GetAll().Where(r => r.GuestId == guestId).ToList();

            double totalCleanlinessRate = guestRates.Sum(r => r.CleanlinessGrade);

            return totalCleanlinessRate / guestRates.Count;    
        }

        public double GetAverageObeyingTheRulesRate(int guestId)
        {
            List<GuestAccommodationRate> guestRates = guestAccommodationRateRepository.GetAll().Where(r => r.GuestId == guestId).ToList();

            double totalCleanlinessRate = guestRates.Sum(r => r.RuleGrade);

            return totalCleanlinessRate / guestRates.Count;
        }

        public Dictionary<int, int> GetRatesForGuest(int guestId)
        {
            List<GuestAccommodationRate> guestRates = guestAccommodationRateRepository.GetAll().Where(r => r.GuestId == guestId).ToList();

            Dictionary<int, int> summedRates = new Dictionary<int, int>();
            for (int i = 1; i <= 5; i++)
            {
                summedRates.Add(i, 0);
            }


            foreach (GuestAccommodationRate guestRate in guestRates)
            {
                switch (guestRate.CleanlinessGrade)
                {
                    case 1:
                        summedRates[1]++; break;
                    case 2:
                        summedRates[2]++; break;
                    case 3:
                        summedRates[3]++; break;
                    case 4:
                        summedRates[4]++; break;
                    case 5:
                        summedRates[5]++; break;
                }

                switch (guestRate.RuleGrade)
                {
                    case 1:
                        summedRates[1]++; break;
                    case 2:
                        summedRates[2]++; break;
                    case 3:
                        summedRates[3]++; break;
                    case 4:
                        summedRates[4]++; break;
                    case 5:
                        summedRates[5]++; break;
                }
            }

            return summedRates;
        }
    }
}
