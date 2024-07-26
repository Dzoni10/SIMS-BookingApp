using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourGuestRepository
    {
        public void Save(TourGuest tourGuest);
        public void SaveAll(List<TourGuest> tourGuests);
        public void Update(TourGuest tourGuest);
        public TourGuest GetById(int id);
        public List<TourGuest> GetAll();
    }
}
