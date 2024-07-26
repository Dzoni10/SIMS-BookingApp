using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.WPF.View;
using iText.StyledXmlParser.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class StatsService
    {
        public readonly double yearDays = 365;
        public AccommodationReservationService accommodationReservationService;
        public EditedReservationsService editedReservationsService;
        public RenovationRecommendationService renovationRecommendationService;
        public ICanceledReservationRepository canceledReservationRepository;
        public StatsService()
        {
            accommodationReservationService = new AccommodationReservationService();
            editedReservationsService = new EditedReservationsService();
            renovationRecommendationService = new RenovationRecommendationService();
            canceledReservationRepository = Injector.Injector.CreateInstance<ICanceledReservationRepository>();
        }
        public List<AccommodationReservation> SortAccommdoationReservations()
        {
            List<AccommodationReservation> accommodationReservations = new List<AccommodationReservation>();
            foreach(AccommodationReservation accommodationReservation in accommodationReservationService.GetAll())
            {
                accommodationReservations.Add(accommodationReservation);
            }
            accommodationReservations = accommodationReservations.OrderBy(reservation => reservation.StartDate.Year).ToList();
            return accommodationReservations;
        }
        public List<StatsDTO> GetStatistics(AccommodationDTO SelectedAccommodation)
        {
            int oldYear = 0;
            List<StatsDTO> accommodationStats = new List<StatsDTO>();
            List<AccommodationReservation> accommodationReservations;
            accommodationReservations = SortAccommdoationReservations();
            foreach(AccommodationReservation accommodationReservation in accommodationReservations)
            {
                int year = accommodationReservation.StartDate.Year;
                if(year != oldYear)
                {
                    int reserved = CountReservations(SelectedAccommodation,year);
                    int canceled = CountCanceled(SelectedAccommodation,year);
                    int rescheduled = CountRescheduling(SelectedAccommodation,year);
                    int advices = CountAdvices(SelectedAccommodation,year);
                    StatsDTO accommodationStat = new StatsDTO(year, reserved,canceled, rescheduled, advices);
                    accommodationStats.Add(accommodationStat);
                    oldYear = accommodationStat.Year;
                }
                else
                {
                    continue;
                }
            }
            return accommodationStats;
        }
        public int CountReservations(AccommodationDTO SelectedAccommodation,int year)
        {
            int reservations = 0;
            foreach(AccommodationReservation accommodationReservation in accommodationReservationService.GetAll())
            {
                if(SelectedAccommodation.Id == accommodationReservation.AccommodationId && accommodationReservation.StartDate.Year == year)
                {
                    reservations++;
                }
            }
            return reservations;
        }
        public int CountCanceled(AccommodationDTO SelectedAccommodation, int year)
        {
            int cancelations = 0;
            foreach (CanceledReservation canceledReservation in canceledReservationRepository.GetAll())
            {
                if (canceledReservation.AccommodationId == SelectedAccommodation.Id && canceledReservation.StartDate.Year == year)
                {
                    cancelations++;
                }
            }
            return cancelations;
        }
        public int CountRescheduling(AccommodationDTO SelectedAccommodation, int year)
        {
            int reschedules = 0;
            foreach (EditedReservation editedReservation in editedReservationsService.GetAll())
            {
                if (editedReservation.AccommodationId == SelectedAccommodation.Id && editedReservation.StartDate.Year == year)
                {
                    if (editedReservation.Status == ReservationStatus.Accepted)
                        reschedules++;
                }
            }
            return reschedules;
        }
        public int CountAdvices(AccommodationDTO SelectedAccommodation, int year)
        {
            int advices = 0;
            foreach(RenovationRecommendation recommendation in renovationRecommendationService.GetAll())
            {
                if(recommendation.RateDate.Year == year && recommendation.AccommodationId==SelectedAccommodation.Id)
                {
                    advices++;
                }
            }
            return advices;
        }
        public Dictionary<int,double> GetBusyYears(AccommodationDTO SelectedAccommodation)
        {
            int year;
            Dictionary<int, double> yearlyBusyness = new Dictionary<int, double>();
            List<AccommodationReservation> accommodationReservations;
            accommodationReservations = SortAccommdoationReservations();
            foreach (AccommodationReservation accommodationReservation in accommodationReservations)
            {
                if (SelectedAccommodation.Id == accommodationReservation.AccommodationId){
                    year = SwitchLongerYear(accommodationReservation.StartDate, accommodationReservation.EndDate);
                    double busyiness = BusyCount(SelectedAccommodation, year);
                    if (!CheckKeyYear(yearlyBusyness, year))
                    {
                        yearlyBusyness.Add(year, busyiness);
                    }
                    else
                    {
                        continue;
                    }
                }   
            }
            return yearlyBusyness;
        }
        public int SwitchLongerYear(DateTime StartDate,DateTime EndDate)
        {
            if (EndDate.Year - StartDate.Year == 1)   
                return EndDate.Year;
            else    
                return StartDate.Year;
        }
        public bool CheckKeyYear(Dictionary<int,double> busyness,int year)
        {
            if (busyness.ContainsKey(year)) { return true; }
            else { return false; }
        }
        public double BusyCount(AccommodationDTO SelectedAccommodation,int year)
        {
            int days = 0;
            List<AccommodationReservation> accommodationReservations;
            accommodationReservations = SortAccommdoationReservations();
            foreach(AccommodationReservation accommodationReservation in accommodationReservations)
            {
                if(SelectedAccommodation.Id == accommodationReservation.AccommodationId && (year == accommodationReservation.StartDate.Year || year == accommodationReservation.EndDate.Year))
                {
                    days += DateInterval(accommodationReservation.StartDate, accommodationReservation.EndDate);
                }
            }
            return days/yearDays;
        }
        public int DateInterval(DateTime startDate,DateTime endDate)
        {
            int day = 0;
            DateTime newYear = new DateTime(startDate.Year, 12, 31);

            if (endDate-newYear >endDate- startDate)
            {
                while (startDate != endDate)
                {
                    day++;
                    startDate = startDate.AddDays(-1);
                }
            }else
            {
                while (startDate != endDate)
                {
                    day++;
                    startDate = startDate.AddDays(1);
                }
            }
            return day;
        }
    }
}
