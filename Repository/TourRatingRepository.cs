using System.Collections.Generic;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;

namespace BookingApp.Repository
{
    public class TourRatingRepository : ITourRatingRepository
    {
        Repository<TourRating> tourRatingRepository;

        public TourRatingRepository()
        {
            tourRatingRepository = new Repository<TourRating>();
        }

        public void Save(TourRating tourRating)
        {
            tourRatingRepository.Save(tourRating);
        }

        public void SaveAll(List<TourRating> tourRatings)
        {
            tourRatingRepository.SaveAll(tourRatings);
        }

        public void Update(TourRating tourRating)
        {
            tourRatingRepository.Update(tourRating);
        }

        public TourRating GetById(int id)
        {
            return tourRatingRepository.FindId(id);
        }

        public List<TourRating> GetAll()
        {
            return tourRatingRepository.GetAll();
        }
    }
}
