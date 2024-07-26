using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;


namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ILocationRepository
    {
        public List<Location> GetAll();
        public void Delete(Location location);
        public Location Save(Location location);
        public void SaveAll(List<Location> locations);
        public void Update(Location location);
        public Location GetById(int id);
        public int SavedLocationId(Location location);

    }
}
