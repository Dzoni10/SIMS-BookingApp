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
    public class OwnerAccommodationRateRepository : IOwnerAccommodationRateRepository
    {
        Repository<OwnerAccommodationRate> ownerAccommodationRateRepository;

        public Subject ownerAccommodationRateSubject;
        public OwnerAccommodationRateRepository()
        {
            ownerAccommodationRateRepository = new Repository<OwnerAccommodationRate>();
            ownerAccommodationRateSubject = new Subject();
        }

        public void Save(OwnerAccommodationRate ownerAccommodationRate)
        {
            ownerAccommodationRateRepository.Save(ownerAccommodationRate);
            ownerAccommodationRateSubject.NotifyObservers();
        }
        public void SaveAll(List<OwnerAccommodationRate> ownerAccommodationRates)
        {
            ownerAccommodationRateRepository.SaveAll(ownerAccommodationRates);
            ownerAccommodationRateSubject.NotifyObservers();
        }
        public void Update(OwnerAccommodationRate ownerAccommodationRate)
        {
            ownerAccommodationRateRepository.Update(ownerAccommodationRate);
            ownerAccommodationRateSubject.NotifyObservers();
        }
        public OwnerAccommodationRate GetById(int id)
        {
            return ownerAccommodationRateRepository.FindId(id);
        }
        public List<OwnerAccommodationRate> GetAll()
        {
            return ownerAccommodationRateRepository.GetAll();
        }
    }
}
