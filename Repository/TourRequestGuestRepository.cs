using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourRequestGuestRepository : ITourRequestGuestRepository
    {
        Repository<TourRequestGuest> tourGuestRepository;

        public TourRequestGuestRepository()
        {
            tourGuestRepository = new Repository<TourRequestGuest>();
        }

        public void Save(TourRequestGuest tourRequestGuest)
        {
            tourGuestRepository.Save(tourRequestGuest);
        }

        public void SaveAll(List<TourRequestGuest> tourRequestGuests)
        {
            tourGuestRepository.SaveAll(tourRequestGuests);
        }

        public void Update(TourRequestGuest tourRequestGuest)
        {
            tourGuestRepository.Update(tourRequestGuest);
        }

        public TourRequestGuest GetById(int id)
        {
            return tourGuestRepository.FindId(id);
        }

        public List<TourRequestGuest> GetAll()
        {
            return tourGuestRepository.GetAll();
        }
        public List<TourRequestGuest> GetByReservationId(int id)
        {
            return tourGuestRepository.GetAll().Where(v => v.TourRequestReservationId == id).ToList();
        }
    }
}
