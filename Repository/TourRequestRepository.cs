using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourRequestRepository : ITourRequestRepository
    {

        Repository<TourRequest> tourRequestRepository;

        public TourRequestRepository()
        {
            tourRequestRepository = new Repository<TourRequest>();
        }

        public void Save(TourRequest tourRequest)
        {
            tourRequestRepository.Save(tourRequest);
        }
        public void SaveAll(List<TourRequest> tourRequest)
        {
            tourRequestRepository.SaveAll(tourRequest);
        }
        public void Update(TourRequest tourRequest)
        {
            tourRequestRepository.Update(tourRequest);
        }
        public TourRequest GetById(int id)
        {
            return tourRequestRepository.FindId(id);
        }
        public List<TourRequest> GetAll()
        {
            return tourRequestRepository.GetAll();
        }

        public List<TourRequest> GetByYear(int year)
        {
            List<TourRequest> list = new List<TourRequest>();
            foreach(TourRequest request in tourRequestRepository.GetAll())
            {
                if(request.DateTo.Year == year)
                    list.Add(request);
            }
            return list;
        }
    }
}
