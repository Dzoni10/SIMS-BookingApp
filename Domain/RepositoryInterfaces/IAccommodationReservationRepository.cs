using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository
    {
        public void Save(AccommodationReservation accommodationReservation);
        public void SaveAll(List<AccommodationReservation> accommodationReservations);
        public void Update(AccommodationReservation accommodationReservation);
        public AccommodationReservation GetById(int id);
        public List<AccommodationReservation> GetAll();
        public void Delete(AccommodationReservation accommodationReservation);
    }
}
