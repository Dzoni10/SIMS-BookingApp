using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.DTO;

namespace BookingApp.Application.UseCases
{
    public class TourRatingService
    {
        private ITourRatingRepository tourRatingRepository;
        public TourRatingService(ITourRatingRepository tourRatingRepository)
        {
            this.tourRatingRepository = tourRatingRepository;
        }

        public List<TourRating> GetAll()
        {
            return tourRatingRepository.GetAll();
        }

        public TourRating GetById(int id)
        {
            return tourRatingRepository.GetById(id);
        }
        public void Update(TourRating rating)
        {
            tourRatingRepository.Update(rating);
        }

        public void Save(TourRating rating)
        {
            tourRatingRepository.Save(rating);
        }

        public bool IsAlreadyReviewed(int startTourDateId, User loggedInUser)
        {
            List<TourRating> tourRatings = tourRatingRepository.GetAll();
            tourRatings = tourRatings.Where(r => r.UserId == loggedInUser.Id).ToList();
            foreach (TourRating rating in tourRatings)
            {
                if (rating.StartTourDateId == startTourDateId)
                    return true;
            }
            return false;
        }

        public List<TourRating> GetAllInLastYear()
        {
            List<TourRating> ratings = new List<TourRating>();
            foreach(var rating in  tourRatingRepository.GetAll())
            {
                if(rating.AssessmentDate.Date >= DateTime.Now.AddYears(-1).Date)
                {
                    ratings.Add(rating);
                }
            }
            return ratings;
        } 
            
    }
}
