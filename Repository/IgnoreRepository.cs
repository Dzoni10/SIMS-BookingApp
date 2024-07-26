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
    public class IgnoreRepository : IIgnoreRepository
    {
        Repository<Ignore> ignoreRepository;

        public Subject ignoreSubject;

        public IgnoreRepository()
        {
            ignoreRepository= new Repository<Ignore>();
            ignoreSubject = new Subject();
        }

        public void Save(Ignore ignore)
        {
            ignoreRepository.Save(ignore);
            ignoreSubject.NotifyObservers();
        }

        public List<Ignore> GetAll()
        {
            return ignoreRepository.GetAll();
        }

        public string GetCommentById(int id)
        {
            foreach(Ignore ignore in ignoreRepository.GetAll())
            {
                if(ignore.EditedReservationId == id)
                {
                    return ignore.Explanation;
                }
            }
            return "//";
        }

        public string GetAccommodationNameById(Accommodation accommodation)
        {
            foreach (Ignore ignore in ignoreRepository.GetAll())
            {
                if (ignore.AccommodationId == accommodation.Id)
                    return accommodation.Name;
            }
            return "//";
        }
    }
}
