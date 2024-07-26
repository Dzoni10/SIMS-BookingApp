using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class TourRequestGuestService
    {
        private ITourRequestGuestRepository tourRequestGuestRepository;
        public TourRequestGuestService(ITourRequestGuestRepository repository)
        {
            tourRequestGuestRepository = repository;
        }
        public void Save(TourRequestGuest tourGuest)
        {
            tourRequestGuestRepository.Save(tourGuest);
        }

        public void Update(TourRequestGuest tourGuest)
        {
            tourRequestGuestRepository.Update(tourGuest);
        }

        public TourRequestGuest GetById(int id)
        {
            return tourRequestGuestRepository.GetById(id);
        }

        public List<TourRequestGuest> GetAll()
        {
            return tourRequestGuestRepository.GetAll();
        }

        public List<TourRequestGuest> GetByRequestId(int id)
        {
            return tourRequestGuestRepository.GetByReservationId(id);
        }
    }
}
