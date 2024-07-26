using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Application.Injector;
using BookingApp.Repository;
using BookingApp.Domain.Models;
using BookingApp.DTO;
using System.Windows.Controls;

namespace BookingApp.Application.UseCases
{
    public class AccommodationsService
    {
        private IAccommodationRepository accommodationRepository;
        public LocationService locationService;
        public List<AccommodationDTO> accommodations;
        public ImageService imageService;

        public AccommodationsService()
        {
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            locationService =new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            accommodations = new List<AccommodationDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
        }

        public int SavedAccommodationId(Accommodation accommodation)
        {
            return accommodationRepository.SavedAccommodationId(accommodation);
        }

        public List<AccommodationDTO> GetAll()
        {
            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                if(!accommodation.IsClosed)
                {
                    accommodation.Location = locationService.GetById(accommodation.LocationId);
                    accommodations.Add(new AccommodationDTO(accommodation));
                }
            }
            return accommodations;
        }
        public List<Accommodation> GetAccommodations()
        {
           return accommodationRepository.GetAll();
        }

        public void Delete(Accommodation accommodation)
        {
            accommodationRepository.Delete(accommodation);
        }
        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }
        public List<AccommodationDTO> GetAccommodationsWithLocation()
        {
            List<Location> locations = locationService.GetAll();
            List<AccommodationDTO> accommodationDTOs = new List<AccommodationDTO>();

            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                Location locationForCurrentAccommodation = locations.FirstOrDefault(loc => loc.Id == accommodation.LocationId);

                if (locationForCurrentAccommodation != null)
                {
                    accommodation.Location = locationForCurrentAccommodation;
                    accommodationDTOs.Add(new AccommodationDTO(accommodation));
                }
            }
            return accommodationDTOs;
        }
        public List<AccommodationDTO> GetAccommodationDTOs()
        {
            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                accommodation.Location = locationService.GetById(accommodation.LocationId);
                accommodation.Image = imageService.GetImageForAccommodation(accommodation.Id);
                accommodations.Add(new AccommodationDTO(accommodation));
            }
            return accommodations;
        }
        public Accommodation GetAccommodationWithLocation(int accomodationId,int locationId)
        {
            Accommodation selectedAccommodation = new Accommodation();
            foreach(Accommodation accommodation in accommodationRepository.GetAll()) 
            {
                if(accommodation.Id==accomodationId && accommodation.LocationId==locationId)
                {
                   return accommodation;
                }   
            }
            return selectedAccommodation;
        }

        public void Update(Accommodation accommodation)
        {
            accommodationRepository.Update(accommodation);
        }
    }
}
