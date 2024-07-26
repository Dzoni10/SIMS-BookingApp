using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel
{
    public class TourReviewViewModel
    {
        private TourReviewService tourReviewService;
        public RelayCommand OkCommand { get;}
        public event EventHandler RequestClose;
        public List<ReviewControl> Reviews { get; set; }
        private int userId;

        public TourReviewViewModel(int userId)
        {
            tourReviewService = new TourReviewService(new UserService(), 
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))),
                new TourRatingService(Injector.CreateInstance<ITourRatingRepository>()),
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), 
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())))),
                new TourGuestService(Injector.CreateInstance<ITourGuestRepository>()),
                new CheckpointService(Injector.CreateInstance<ICheckpointRepository>()));
            Reviews = new List<ReviewControl>();
            OkCommand = new RelayCommand(Close);
            this.userId = userId;
            SetReviewContol();
        }
        private void SetReviewContol()
        {
            foreach(ReviewControl reviewControl in tourReviewService.GetReviewControls(userId))
            {
                Reviews.Add(reviewControl);
            }
        }

        private void Close()
        {
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
    }
}
