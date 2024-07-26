using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.Observer;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        Repository<AccommodationReservation> reservationRepository;
        public Subject accommodationReservationSubject;
        public AccommodationReservationRepository()
        {
            reservationRepository = new Repository<AccommodationReservation>();
            accommodationReservationSubject = new Subject();
        }

        public void Save(AccommodationReservation accommodationReservation)
        {
            reservationRepository.Save(accommodationReservation);
            accommodationReservationSubject.NotifyObservers();
        }

        public void SaveAll(List<AccommodationReservation> accommodationReservations)
        {
            reservationRepository.SaveAll(accommodationReservations);
            accommodationReservationSubject.NotifyObservers();
        }
        public void Update(AccommodationReservation accommodationReservation)
        {
            reservationRepository.Update(accommodationReservation);
            accommodationReservationSubject.NotifyObservers();
        }
        public AccommodationReservation GetById(int id)
        {
            return reservationRepository.FindId(id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return reservationRepository.GetAll();
        }

        
        public void Delete(AccommodationReservation accommodationReservation)
        {
            reservationRepository.Delete(accommodationReservation);
            accommodationReservationSubject.NotifyObservers();
        }
    }
}
