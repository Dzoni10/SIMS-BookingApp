using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {
        public void Save(TourRequest tourRequest);
        public void SaveAll(List<TourRequest> tourRequest);
        public void Update(TourRequest tourRequest);
        public TourRequest GetById(int id);
        public List<TourRequest> GetAll();
        public List<TourRequest> GetByYear(int year);
    }
}
