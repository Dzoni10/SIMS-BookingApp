using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class TourReviewService
    {
        private UserService userService;
        private TourService tourService;
        private TourRatingService ratingService;
        private StartTourDateService startTourDateService;
        private TourReservationService tourReservationService;
        private TourGuestService tourGuestService;
        private CheckpointService checkpointService;
        public TourReviewService(UserService userService, TourService tourService, TourRatingService ratingService, StartTourDateService startTourDateService, TourReservationService tourReservationService, TourGuestService tourGuestService, CheckpointService checkpointService) 
        {
            this.userService = userService;
            this.tourService = tourService;
            this.ratingService = ratingService;
            this.startTourDateService = startTourDateService;
            this.tourReservationService = tourReservationService;
            this.tourGuestService = tourGuestService;
            this.checkpointService = checkpointService;
        }

        public List<TourRating> GetByGuideId(int guideId)
        {
            List<TourRating> ratings = new List<TourRating>();
            foreach (TourRating rating in ratingService.GetAll())
            {
                if (guideId == tourService.GetById(startTourDateService.GetById(rating.StartTourDateId).TourId).UserId)
                {
                    ratings.Add(rating);
                }
            }
            return ratings;
        }

        public string GenerateHint(int startTourId, int guideId)
        {
            string hint = "Tourists joined at the following points:\n";
            foreach (var tourReservation in tourReservationService.GetAll())
            {
                if (tourReservation.User.Id == guideId && tourReservation.StartTourDateId == startTourId)
                {
                    hint += FindTourGuests(tourReservation.Id);
                }
            }
            return hint;
        }

        private string FindTourGuests(int reservationId)
        {
            string hint = string.Empty;
            foreach (TourGuest guest in tourGuestService.GetAll())
            {
                if (guest.TourReservationId == reservationId)
                {
                    if (guest.CheckPointId != -1)
                        hint += guest.FullName + "-" + checkpointService.GetById(guest.CheckPointId).Name + "\n";
                    else
                        hint += guest.FullName + "-" + "he didn't come" + "\n";
                }
            }
            return hint;
        }

        public List<ReviewControl> GetReviewControls(int giudeId)
        {
            List<ReviewControl> reviews = new List<ReviewControl>(); 
            foreach (TourRating rating in GetByGuideId(giudeId))
            {
                var reviewControl = new ReviewControl(rating.Id, userService.GetById(rating.UserId).Username, rating.GuidesKnowledge, rating.GuidesLanguageAbility,
                    rating.TourExcitement, rating.Comment, GenerateHint(rating.StartTourDateId, rating.UserId),
                    tourService.GetById(startTourDateService.GetById(rating.StartTourDateId).TourId).Name, rating.Valid);
                reviews.Add(reviewControl);
            }
            return reviews;
        }
    }
}
