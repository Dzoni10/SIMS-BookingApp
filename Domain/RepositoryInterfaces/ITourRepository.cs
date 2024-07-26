using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public Tour Save(Tour tour);
        public void SaveAll(List<Tour> tours);
        public void Update(Tour tour);
        public Tour GetById(int id);
        public List<Tour> GetAll();
    }
}
