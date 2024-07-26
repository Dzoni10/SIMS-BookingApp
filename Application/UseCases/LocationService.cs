using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BookingApp.DTO;
using BookingApp.Model;

namespace BookingApp.Application.UseCases
{
    public class LocationService
    {
        public ILocationRepository locationRepository;
        public LocationService(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public Location Save(Location location)
        {
            return locationRepository.Save(location);
        }

        public int SavedLocationId(Location location)
        {
            return locationRepository.SavedLocationId(location);
        }
        public Location GetById(int id)
        {
            return locationRepository.GetById(id);
        }
        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }
        public List<Location> GetUniqueStates()
        {
            List<Location> locations = new List<Location>();
            HashSet<string> uniqueStates = new HashSet<string>();

            foreach (Location location in locationRepository.GetAll())
            {
                if (!uniqueStates.Contains(location.State))
                {
                    uniqueStates.Add(location.State);
                    locations.Add(location);
                }
            }
            return locations;
        }

        public List<Location> GetUniqueCities()
        {
            List<Location> locations = new List<Location>();
            HashSet<string> uniqueCities = new HashSet<string>();

            foreach (Location location in locationRepository.GetAll())
            {
                if (!uniqueCities.Contains(location.City))
                {
                    uniqueCities.Add(location.City);
                    locations.Add(location);
                }
            }
            return locations;
        }

        public List<string> GetCitiesForState(string selectedState)
        {
            List<string> cities = new List<string>();
            if (string.IsNullOrEmpty(selectedState))
            {
                return new List<string>();
            }
            else
            {
                return locationRepository.GetAll().Where(location => location.State.Equals(selectedState)).
                Select(location => location.City).Distinct().ToList();
            }
        }

        public List<Tour> SearchByCity(List<Tour> tours, string city)
        {
            if (!string.IsNullOrEmpty(city))
            {
                tours = tours.Where(t => t.Location.City.ToLower() == city.ToLower()).ToList();
            }

            return tours;
        }

        public List<Tour> SearchByState(List<Tour> tours, string state)
        {
            if (!string.IsNullOrEmpty(state))
            {
                tours = tours.Where(t => t.Location.State.ToLower() == state.ToLower()).ToList();
            }

            return tours;
        }

        public bool IsLocationAlreadyWritten(string state, string city)
        {
            Location location = locationRepository.GetAll().FirstOrDefault(l => l.State.Equals(state) && l.City.Equals(city));
            if (location == null)
                return false;
            return true;
        }

        public Location GetLocation(string state, string city)
        {
            Location location = locationRepository.GetAll().FirstOrDefault(l => l.State.Equals(state) && l.City.Equals(city));
            return location;
        }
        public List<Location> GetLocationsByState(string state)
        {
            return locationRepository.GetAll().Where(l => l.State.Equals(state)).ToList();
        }
        public List<Location> GetLocationsByCity(string city)
        {
            return locationRepository.GetAll().Where(l => l.City.Equals(city)).ToList();
        }
        public Location GetLocationForAccommodation(int accommodationId)
        {
            List<Location> locations = locationRepository.GetAll();
            foreach (Location location in locations)
            {
                if (location.Id == accommodationId)
                {
                    return location;
                }
            }
            return null;
        }
    }
}
