using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;

namespace BookingApp.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        Repository<Accommodation> accommodationRepository;

        public Subject accommodationSubject;
        public AccommodationRepository()
        {
            accommodationRepository = new Repository<Accommodation>();
            accommodationSubject = new Subject();   
        }

       
        public int SavedAccommodationId(Accommodation accommodation)
        {
            accommodationRepository.Save(accommodation);
            accommodationSubject.NotifyObservers();
            return accommodation.Id;
        }

        public void SaveAll(List<Accommodation> accommodations)
        {
            accommodationRepository.SaveAll(accommodations);
            accommodationSubject.NotifyObservers();
        }

        public void Update(Accommodation accommodation)
        {
            accommodationRepository.Update(accommodation);
            accommodationSubject.NotifyObservers();
        }

        public Accommodation GetById(int id)
        {
            return accommodationRepository.FindId(id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodationRepository.GetAll();
        }
        public void Delete(Accommodation accommodation)
        {
            accommodationRepository.Delete(accommodation);
            accommodationSubject.NotifyObservers();
        }
       
    }
}
