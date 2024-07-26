using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
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
    public class TouristAttendingNotificationViewModel
    {
        public TouristAttendingNotification parentWindow { get; set; }
        public ObservableCollection<TourGuestDTO> SignedUpTourists { get; set; }
        public int StartTourDateId;
        public int NotificationId { get; set; }
        private TourService TourService;
        private StartTourDateService StartTourDateService;
        private TourGuestService TourGuestService;
        private TourReservationService TourReservationService;
        public User LoggedInUser { get; set; }
        private TouristNotificationWindowViewModel ParentViewModel;
        public RelayCommand DeleteCommand { get; set; }

        public TouristAttendingNotificationViewModel(TouristAttendingNotification touristAttendingNotification, User user, int startTourDateId, TouristNotificationWindowViewModel parentViewModel)
        {
            parentWindow = touristAttendingNotification;
            NotificationId = parentWindow.NotificationId;
            SignedUpTourists = new ObservableCollection<TourGuestDTO>();
            StartTourDateId = startTourDateId;
            LoggedInUser = user;
            ParentViewModel = parentViewModel;
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            StartTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            TourGuestService = new TourGuestService(Injector.CreateInstance<ITourGuestRepository>());
            TourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), TourService);
            Update();
            InitializeLabel(StartTourDateId);

            DeleteCommand = new RelayCommand(DeleteClick);
        }

        private void Update()
        {
            List<TourReservation> reservations = TourReservationService.GetAll().Where(tr => tr.User.Id == LoggedInUser.Id).ToList();
            foreach (TourReservation reservation in reservations)
            {
                foreach (TourGuest possibleGuest in TourGuestService.GetAll())
                {
                    if (reservation.StartTourDateId == StartTourDateId && possibleGuest.TourReservationId == reservation.Id && possibleGuest.CheckPointId != -1)
                    {
                        SignedUpTourists.Add(new TourGuestDTO(possibleGuest));
                    }
                }
            }
        }

        private void DeleteClick()
        {
            ParentViewModel.DeleteTouristAttendingNotification(parentWindow);
        }

        private void InitializeLabel(int startTourDateId)
        {
            StartTourDate finishedTour = StartTourDateService.GetById(startTourDateId);
            Tour tour = TourService.GetById(finishedTour.TourId);
            parentWindow.TourNameLabel.Content = $"{tour.Name} {finishedTour.Date.ToString("dd.MM.yyyy HH:mm")}";
        }
    }
}
