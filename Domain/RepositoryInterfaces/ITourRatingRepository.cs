using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRatingRepository
    {
        public void Save(TourRating tourRating);
        public void SaveAll(List<TourRating> tourRatings);
        public void Update(TourRating tourRating);
        public TourRating GetById(int id);
        public List<TourRating> GetAll();
    }
}
