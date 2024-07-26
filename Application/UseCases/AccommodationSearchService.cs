using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.Application.UseCases
{
    public class AccommodationSearchService
    {
        private AccommodationsService accommodationService;
        public AccommodationSearchService() {
            accommodationService = new AccommodationsService();
        }
        public List<AccommodationDTO> FilterAccommodationsByName(List<AccommodationDTO> accommodationsDTO, string accommodationName)
        {
            accommodationName = accommodationName.Trim();

            string[] words = accommodationName.Split(' ');
            if (words.Length == 1)
            {
                accommodationsDTO = accommodationsDTO
                    .Where(accommodation => accommodation.Name.ToLower().Contains(words[0].ToLower()))
                    .ToList();

            }
            return accommodationsDTO;
        }
        public List<AccommodationDTO> FilterAccommodationsByState(List<AccommodationDTO> accommodationsDTO, string state)
        {
            accommodationsDTO = accommodationsDTO
                .Where(accommodation => accommodation.Location.State.Equals(state))
                .ToList();

            return accommodationsDTO;
        }

        public List<AccommodationDTO> FilterAccommodationsByCity(List<AccommodationDTO> accommodationsDTO, string city)
        {
            accommodationsDTO = accommodationsDTO
                .Where(accommodation => accommodation.Location.City.Equals(city))
                .ToList();

            return accommodationsDTO;
        }
        public List<AccommodationDTO> FilterAccommodationsByType(List<AccommodationDTO> accommodationsDTO, string selectedType)
        {
            AccommodationType type = (AccommodationType)Enum.Parse(typeof(AccommodationType), selectedType);

            accommodationsDTO = accommodationsDTO
                .Where(accommodation => accommodation.Type == type)
                .ToList();

            return accommodationsDTO;
        }
        public List<AccommodationDTO> FilterAccommodationsByNumberOfGuestsDTOs(List<AccommodationDTO> accommodationsDTO, string numberOfGuests)
        {
            int guestNumber = int.Parse(numberOfGuests);
            accommodationsDTO = accommodationsDTO
                .Where(accommodation => accommodation.Capacity >= guestNumber)
                .ToList();

            return accommodationsDTO;
        }
        public List<Accommodation> FilterAccommodationsByNumberOfGuests(int numberOfGuests)
        {
            List<Accommodation> accommodations = accommodationService.GetAccommodations();
            accommodations = accommodations
                .Where(accommodation => accommodation.Capacity >= numberOfGuests)
                .ToList();

            return accommodations;
        }
        public List<AccommodationDTO> FilterAccommodationsByDuration(List<AccommodationDTO> accommodationsDTO, string reservationDuration)
        {
            int stayDays = int.Parse(reservationDuration);
            accommodationsDTO = accommodationsDTO
                .Where(accommodation => accommodation.MinStayDays >= stayDays)
                .ToList();

            return accommodationsDTO;
        }
    }
}
