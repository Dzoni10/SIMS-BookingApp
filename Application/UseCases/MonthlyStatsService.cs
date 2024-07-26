using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using BookingApp.Domain.Models;

namespace BookingApp.Application.UseCases
{
    public class MonthlyStatsService
    {
        public AccommodationReservationService accommodationReservationService;
        public CanceledReservationService canceledReservationService;
        public EditedReservationsService editedReservationsService;
        public RenovationRecommendationService renovationRecommendationService;
        public MonthlyStatsService()
        {
            accommodationReservationService = new AccommodationReservationService();
            canceledReservationService = new CanceledReservationService();
            editedReservationsService = new EditedReservationsService();
            renovationRecommendationService = new RenovationRecommendationService();
        }

        public List<StatsDTO> GetMonthStats(int year,int accommodationId)
        {
            List<StatsDTO> accommodationStats = new List<StatsDTO>();
            int i = 1;
            for(i=1;i<=12;i++)
            {
                Month month = (Month)i;

                int reserved = CountMonthReservations(year,accommodationId,month);
                int canceled = CountMonthCancelations(year, accommodationId, month);
                int rescheduled = CountMonthRescheduling(year, accommodationId, month);
                int advices = CountMonthAdvices(year, accommodationId, month);
                StatsDTO accommodationStat = new StatsDTO(month, reserved, canceled, rescheduled, advices);
                accommodationStats.Add(accommodationStat);
            }
            return accommodationStats;
        }
        private int CountMonthReservations(int year,int accommodationId,Month month) {
            int monthlyReservation = 0;
            foreach(AccommodationReservation accommodationsReservation in accommodationReservationService.GetAll())
            {
                if(accommodationsReservation.StartDate.Month == (int)month && accommodationsReservation.StartDate.Year == year && accommodationsReservation.AccommodationId == accommodationId)
                { monthlyReservation++; }
            }
            return monthlyReservation;
        }
        private int CountMonthCancelations(int year, int accommodationId, Month month) {
            int monthlyCancelation = 0;
            foreach (CanceledReservation canceledReservation in canceledReservationService.GetAll())
            {
                if (canceledReservation.StartDate.Month == (int)month && canceledReservation.StartDate.Year == year && canceledReservation.AccommodationId == accommodationId)
                { monthlyCancelation++; }
            }
            return monthlyCancelation;
        }
        private int CountMonthRescheduling(int year, int accommodationId, Month month) {
            int monthlyReschedule = 0;
            foreach (EditedReservation editedReservation in editedReservationsService.GetAll())
            {
                if (editedReservation.StartDate.Month == (int)month && editedReservation.StartDate.Year == year && editedReservation.AccommodationId == accommodationId)
                { monthlyReschedule++; }
            }
            return monthlyReschedule;
        }
        private int CountMonthAdvices(int year, int accommodationId, Month month) {
            int monthlyAdvice=0;
            foreach(RenovationRecommendation recommendation in renovationRecommendationService.GetAll())
            {
                if(recommendation.AccommodationId == accommodationId && recommendation.RateDate.Year == year &&recommendation.RateDate.Month == (int)month)
                {
                    monthlyAdvice++;
                }
            }
            return monthlyAdvice++;
        }
    }
}
