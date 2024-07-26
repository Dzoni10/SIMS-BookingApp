using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class AvailableReservationIntervalsService
    {
        private AccommodationReservationService accommodationReservationService;

        public AvailableReservationIntervalsService()
        {
            accommodationReservationService = new AccommodationReservationService();
        }
        public List<AccommodationReservation> GetExistingReservations(int selectedAccommodationId)
        {
            return accommodationReservationService
                .GetAll()
                .Where(reservation => reservation.AccommodationId == selectedAccommodationId)
                .OrderBy(x => x.StartDate).ToList();
        }
        public List<AccommodationReservation> GetAvailableTimeSpans(int selectedAccommodationId, AccommodationReservationDTO selectedAccommodationReservation)
        {
            List<AccommodationReservation> availableTimeSpans = new List<AccommodationReservation>();
            List<AccommodationReservation> existingReservations = GetExistingReservations(selectedAccommodationId);

            if (existingReservations.Count > 0)
            {
                AddAvailableTimeSpan(selectedAccommodationReservation.StartDate, existingReservations.First().StartDate, availableTimeSpans);

                AccommodationReservation previousReservation = existingReservations.First();
                foreach (AccommodationReservation reservation in existingReservations)
                {
                    AddAvailableTimeSpan(previousReservation.EndDate, reservation.StartDate, availableTimeSpans);
                    previousReservation = reservation;
                }
                AddAvailableTimeSpan(existingReservations.Last().EndDate, selectedAccommodationReservation.EndDate, availableTimeSpans);
            }
            else
            {
                AddAvailableTimeSpan(selectedAccommodationReservation.StartDate, selectedAccommodationReservation.EndDate, availableTimeSpans);
            }
            return availableTimeSpans;
        }
        private void AddAvailableTimeSpan(DateTime start, DateTime end, List<AccommodationReservation> availableTimeSpans)
        {
            availableTimeSpans.Add(new AccommodationReservation
            {
                StartDate = start,
                EndDate = end
            });
        }
        public List<AccommodationReservation> TimeSpansGreaterThanMinStayDays(List<AccommodationReservation> timeSpans, int minStayDays)
        {
            List<AccommodationReservation> matchingTimeSpans = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in timeSpans)
            {
                if (reservation.EndDate.Subtract(reservation.StartDate).Days >= minStayDays)
                {
                    matchingTimeSpans.Add(new AccommodationReservation
                    {
                        StartDate = reservation.StartDate,
                        EndDate = reservation.EndDate
                    });
                }
            }
            return matchingTimeSpans;
        }
        public List<AccommodationReservationDTO> SplitTimeSpansToDays(List<AccommodationReservation> timeSpans, int minStayDays)
        {
            List<AccommodationReservationDTO> daySpans = new List<AccommodationReservationDTO>();

            foreach (AccommodationReservation reservation in timeSpans)
            {
                DateTime currentDate = reservation.StartDate;
                daySpans.AddRange(GetReservationOption(reservation.StartDate, reservation.EndDate, minStayDays));
            }
            return daySpans;
        }
        public List<AccommodationReservationDTO> GetReservationOption(DateTime currentDate, DateTime reservationEndDate, int minStayDays)
        {
            List<AccommodationReservationDTO> reservationOptions = new List<AccommodationReservationDTO>();

            while (currentDate <= reservationEndDate.AddDays(-minStayDays + 1))
            {
                if (currentDate.AddDays(minStayDays) <= reservationEndDate.AddDays(1))
                {
                    reservationOptions.Add(new AccommodationReservationDTO
                    {
                        StartDate = currentDate,
                        EndDate = currentDate.AddDays(minStayDays)
                    });
                }
                currentDate = currentDate.AddDays(1);
            }
            return reservationOptions;
        }
        public List<AccommodationReservationDTO> GetAvailableReservationOptions(int selectedAccommodationId, AccommodationReservationDTO accommodationReservationDTO)
        {
            List<AccommodationReservation> calculatedTimeSpans = GetAvailableTimeSpans(selectedAccommodationId, accommodationReservationDTO);
            calculatedTimeSpans = TimeSpansGreaterThanMinStayDays(calculatedTimeSpans, accommodationReservationDTO.DaysReserved);
            List<AccommodationReservationDTO> daySpans = SplitTimeSpansToDays(calculatedTimeSpans, accommodationReservationDTO.DaysReserved);
            return daySpans;
        }
    }
}
