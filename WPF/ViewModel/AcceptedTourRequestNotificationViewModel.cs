using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel
{
    public class AcceptedTourRequestNotificationViewModel
    {
        public AcceptedTourRequestNotification parentWindow { get; set; }
        public User LoggedInUser { get; set; }
        public int RequestId { get; set; }
        public int NotificationId { get; set; }
        private TourRequestService TourRequestService { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        private TouristNotificationWindowViewModel ParentViewModel;

        public AcceptedTourRequestNotificationViewModel(AcceptedTourRequestNotification window, User user, int requestId, TouristNotificationWindowViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
            parentWindow = window;
            LoggedInUser = user;
            NotificationId = parentWindow.NotificationId;
            RequestId = requestId;
            TourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            DeleteCommand = new RelayCommand(DeleteClick);
            InitializeLabel();
        }
    
        private void InitializeLabel()
        {
            TourRequest request = TourRequestService.GetById(RequestId);
            parentWindow.DetailsLabel.Content = $"Location: {request.Location.City}, {request.Location.State}. Date: {request.DateIfAccepted.ToString("dd.MM.yyyy HH:mm")}";
        }

        private void DeleteClick()
        {
            ParentViewModel.DeleteAcceptedNotification(parentWindow);
        }
    }
}
