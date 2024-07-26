using System.Collections.Generic;

using BookingApp.Domain.RepositoryInterfaces;

using System.Linq;

using BookingApp.Model;

namespace BookingApp.Repository
{
    public class TourGuestRepository : ITourGuestRepository
    {
        Repository<TourGuest> tourGuestRepository;

        public TourGuestRepository()
        {
            tourGuestRepository = new Repository<TourGuest>();
        }

        public void Save(TourGuest tourGuest)
        {
            tourGuestRepository.Save(tourGuest);
        }

        public void SaveAll(List<TourGuest> tourGuests)
        {
            tourGuestRepository.SaveAll(tourGuests);
        }

        public void Update(TourGuest tourGuest)
        {
            tourGuestRepository.Update(tourGuest);
        }

        public TourGuest GetById(int id)
        {
            return tourGuestRepository.FindId(id);
        }

        public List<TourGuest> GetAll()
        {
            return tourGuestRepository.GetAll();
        }
        public List<TourGuest> GetByReservationId(int id)
        {
            return tourGuestRepository.GetAll().Where(v => v.TourReservationId == id).ToList();
        }
    }
}
