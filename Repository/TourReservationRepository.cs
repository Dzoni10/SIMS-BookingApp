using System.Collections.Generic;

using BookingApp.Domain.RepositoryInterfaces;

using System.Linq;

using BookingApp.Model;

namespace BookingApp.Repository
{
    public class TourReservationRepository : ITourReservationRepository
    {
        Repository<TourReservation> tourReservationRepository;

        public TourReservationRepository()
        {
            tourReservationRepository = new Repository<TourReservation>();
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            return tourReservationRepository.Save(tourReservation);
        }

        public void SaveAll(List<TourReservation> tourReservations)
        {
            tourReservationRepository.SaveAll(tourReservations);
        }

        public void Update(TourReservation tourReservation)
        {
            tourReservationRepository.Update(tourReservation);
        }

        public TourReservation GetById(int id)
        {
            return tourReservationRepository.FindId(id);
        }

        public List<TourReservation> GetAll()
        {
            return tourReservationRepository.GetAll();
        }

        public List<TourReservation> GetByStartTourDateId(int id)
        {
            return tourReservationRepository.GetAll()
            .Where(tourReservation => tourReservation.StartTourDateId == id)
            .ToList();

        }
    }
}
