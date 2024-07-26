using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using Microsoft.VisualBasic;


namespace BookingApp.Repository
{
    public class LocationRepository : ILocationRepository
    {
        Repository<Location> locationRepository;
        public LocationRepository()
        {
            locationRepository = new Repository<Location>();
        }

        public List<Location> GetAll() 
        {
            return locationRepository.GetAll();
        }

        public void Delete(Location location)
        {
            locationRepository.Delete(location);
        }


        public Location Save(Location location)
        {
            return locationRepository.Save(location);
            
        }
        
        public void SaveAll(List<Location> locations)
        {
            locationRepository.SaveAll(locations);
        }
        public void Update(Location location)
        {
            locationRepository.Update(location);
        }

        public Location GetById(int id)
        {
            return locationRepository.FindId(id);
        }
      
        public int SavedLocationId(Location location)
        {
            locationRepository.Save(location);
            return location.Id;
        }

    }
}
