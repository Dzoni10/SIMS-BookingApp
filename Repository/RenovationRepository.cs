using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Observer;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class RenovationRepository : IRenovationRepository
    {
        Repository<Renovation> renovationRepository;
        public Subject renovationSubject;

        public RenovationRepository()
        {
            renovationSubject = new Subject();
            renovationRepository = new Repository<Renovation>();
        }

        public void Save(Renovation renovation)
        {
            renovationRepository.Save(renovation);
            renovationSubject.NotifyObservers();
        }

        public Renovation GetById(int id)
        {
            return renovationRepository.FindId(id);
        }

        public void Update(Renovation renovation)
        {
            renovationRepository.Update(renovation);
            renovationSubject.NotifyObservers();
        }

        public void Delete(Renovation renovation)
        {
            renovationRepository.Delete(renovation);
            renovationSubject.NotifyObservers();
        }
        public List<Renovation> GetAll()
        {
            return renovationRepository.GetAll();
        }
    }
}
