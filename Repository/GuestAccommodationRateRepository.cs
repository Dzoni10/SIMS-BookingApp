using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class GuestAccommodationRateRepository : IGuestAccommodationRateRepository
    {

        Repository<GuestAccommodationRate> guestAccommodationRateRepository;

        public Subject guestAccommodationRateSubject;
        public GuestAccommodationRateRepository()
        {
            guestAccommodationRateRepository = new Repository<GuestAccommodationRate>();
            guestAccommodationRateSubject = new Subject();
        }

        public void Save(GuestAccommodationRate guestAccommodationRate)
        {
            guestAccommodationRateRepository.Save(guestAccommodationRate);
            guestAccommodationRateSubject.NotifyObservers();
        }

        public void SaveAll(List<GuestAccommodationRate> guestAccommodationRates)
        {
            guestAccommodationRateRepository.SaveAll(guestAccommodationRates);
            guestAccommodationRateSubject.NotifyObservers();
        }
        public void Update(GuestAccommodationRate guestAccommodationRate)
        {
            guestAccommodationRateRepository.Update(guestAccommodationRate);
            guestAccommodationRateSubject.NotifyObservers();
        }

        public GuestAccommodationRate GetById(int id)
        {
            return guestAccommodationRateRepository.FindId(id); 
        }
        public List<GuestAccommodationRate> GetAll()
        {
            return guestAccommodationRateRepository.GetAll();
        }
    }

}
