using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.Domain.Models;

namespace BookingApp.Application.UseCases
{
    public class RenovationService
    {
        private readonly int givenDates=3;
        public List<DateRangeDTO> availableDates;
        public List<DateRangeDTO> outRangeDates;
        public List<RenovationDTO> renovations;
        public AccommodationReservationService accommodationReservationService;
        public IRenovationRepository renovationRepository;
        public AccommodationsService accommodationsService;
        public StatsService statsService;
        public RenovationService()
        {
            availableDates = new List<DateRangeDTO>();
            outRangeDates = new List<DateRangeDTO>();
            renovations = new List<RenovationDTO>();
            accommodationReservationService = new AccommodationReservationService();
            renovationRepository = Injector.Injector.CreateInstance<IRenovationRepository>();
            accommodationsService = new AccommodationsService();
            statsService =  new StatsService();
        }
        public List<DateRange> GetReservedDates(AccommodationDTO SelectedAccommodation)
        {
            List<DateRange> reservedDates = new List<DateRange>();
            List<AccommodationReservation> accommodationReservations = statsService.SortAccommdoationReservations();
            foreach (AccommodationReservation accommodationReservation in accommodationReservations)
            {
                if (SelectedAccommodation.Id == accommodationReservation.AccommodationId)
                    reservedDates.Add(new DateRange(accommodationReservation.StartDate, accommodationReservation.EndDate));
            }
            return reservedDates;
        }
        public bool IsDateOutRange(DateTime startDate, DateTime endDate,DateRange reservedDate) 
        {
            if ((startDate >= reservedDate.StartDate && startDate < reservedDate.EndDate) || (endDate > reservedDate.StartDate && endDate <= reservedDate.EndDate))
            {
                return true;
            }
            else if (startDate < reservedDate.StartDate && endDate > reservedDate.EndDate)
                return true;
            else
                return false;
        }
        public bool IsDateOverlaps(AccommodationDTO SelectedAccommodation, DateTime startDate,DateTime endDate)
        {
            List<DateRange> reservedDates = GetReservedDates(SelectedAccommodation);
            foreach(DateRange reserved in reservedDates)
            {
                if(IsDateOutRange(startDate, endDate, reserved))
                    return true;
            }
            return false;
        }
        public List<DateRange> AvailableDates(AccommodationDTO SelectedAccommodation,DateTime startDate,DateTime endDate)
        {
            List<DateRange> availableDates = new List<DateRange>();
            DateTime start = startDate;
            DateTime date;
            DateTime end = endDate;
            for (int i = 0; i < givenDates; i++)
            {
                while (IsDateOverlaps(SelectedAccommodation, start, end))
                {
                    start = start.AddDays(1);
                    end = end.AddDays(1);
                }
                availableDates.Add(new DateRange(start, end));
                date = start;
                start= end;
                end= end.AddDays((int)(end-date).TotalDays);
            }
            return availableDates;
        }
        public void SaveRenovation(Renovation renovation)
        {
            renovationRepository.Save(renovation);
        }
        public List<RenovationDTO> GetAllRenovations()
        {
            foreach(Renovation renovation in renovationRepository.GetAll())
            {
                renovation.Accommodation = accommodationsService.GetById(renovation.AccommodationId);
                renovations.Add(new RenovationDTO(renovation));
            }
            return renovations;   
        }
        public void CancelRenovation(RenovationDTO renovation)
        {
            renovationRepository.Delete(renovation.ToRenovation());
        }
    }
}
