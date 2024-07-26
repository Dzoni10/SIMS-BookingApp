using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class TourGuestService
    {
        private ITourGuestRepository tourGuestRepository;
        public TourGuestService(ITourGuestRepository tourGuestRepository)
        {
            this.tourGuestRepository = tourGuestRepository;
        }
        public void Save(TourGuest tourGuest)
        {
            tourGuestRepository.Save(tourGuest);
        }

        public void Update(TourGuest tourGuest)
        {
            tourGuestRepository.Update(tourGuest);
        }

        public TourGuest GetById(int id)
        {
            return tourGuestRepository.GetById(id);
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
