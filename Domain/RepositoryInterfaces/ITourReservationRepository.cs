using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        public TourReservation Save(TourReservation tourReservation);
        public void SaveAll(List<TourReservation> tourReservations);
        public void Update(TourReservation tourReservation);
        public TourReservation GetById(int id);
        public List<TourReservation> GetAll();
    }
}
