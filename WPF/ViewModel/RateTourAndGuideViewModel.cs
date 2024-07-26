using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel
{
    public class RateTourAndGuideViewModel
    {

        public RateTourAndGuide parentWindow { get; set; }

        public ObservableCollection<StartTourDateDTO> Tours { get; set; }
        public ObservableCollection<RateTourControl> Ratings { get; set; }
        public List<TourReservation> TourReservations { get; set; }
        public List<TourRating> TourRatings { get; set; }
        private TourService TourService;
        private TourRatingService TourRatingService;
        private TourReservationService TourReservationService;
        private StartTourDateService StartTourDateService;
        public User LoggedInUser { get; set; }
        private readonly NotificationManager _notificationManager = new NotificationManager(NotificationPosition.TopRight);
        public RelayCommand TutorialCommand { get; set; }
        public RateTourAndGuideViewModel(RateTourAndGuide rateTourAndGuide, User user)
        {
            parentWindow = rateTourAndGuide;
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            TourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), TourService);
            StartTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            TourRatingService = new TourRatingService(Injector.CreateInstance<ITourRatingRepository>());
            TourReservations = TourReservationService.GetAll();
            TourRatings = TourRatingService.GetAll();
            LoggedInUser = user;
            Tours = new ObservableCollection<StartTourDateDTO>();
            Ratings = new ObservableCollection<RateTourControl>();
            TutorialCommand = new RelayCommand(TutorialClick);
            Update();
            SetReviewControl();
        }

        private void Update()
        {
            TourReservations = TourReservations.Where(t => t.User.Id == LoggedInUser.Id).ToList();
            foreach (TourReservation reservation in TourReservations)
            {
                StartTourDate tour = StartTourDateService.GetById(reservation.StartTourDateId);
                bool alreadyReviewed = TourRatingService.IsAlreadyReviewed(reservation.StartTourDateId, LoggedInUser);
                if (tour.Status == TourStatus.Finished && !alreadyReviewed)
                {
                    Tours.Add(new StartTourDateDTO(tour));
                }
            }
        }

        private void TutorialClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new TutorialStatistics("C:\\Users\\nikol\\Desktop\\Nikola\\Random slike i videi xd\\Random slike i videi xd\\Tutorijali\\RatingTutorial.mkv"));
        }

        private void SetReviewControl()
        {
            foreach (var reservation in Tours)
            {
                Tour tour = TourService.GetById(reservation.TourId);
                string name = tour.Name + " " + reservation.Date.ToString("dd.MM.yyyy HH:mm");
                var ratings = new RateTourControl(name, LoggedInUser, reservation.TourId);
                Ratings.Add(ratings);
            }
        }

        private NotificationContent SetNotificationMessage(NotificationContent content)
        {
            return content;
        }

        private void OnDeleteRequested(object sender, EventArgs e)
        {
            // Pronađite ListViewItem koji sadrži određeni RateTourControl
            
        }
    }
}
