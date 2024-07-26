using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRequestGuestRepository
    {
        public void Save(TourRequestGuest tourRequestGuest);
        public void SaveAll(List<TourRequestGuest> tourRequestGuests);
        public void Update(TourRequestGuest tourRequestGuest);
        public TourRequestGuest GetById(int id);
        public List<TourRequestGuest> GetAll();
        public List<TourRequestGuest> GetByReservationId(int id);
    }
}
