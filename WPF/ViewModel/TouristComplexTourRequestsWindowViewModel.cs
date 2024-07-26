using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel
{
    public class TouristComplexTourRequestsWindowViewModel
    {
        public TouristComplexTourRequestsWindow parentWindow { get; set; }
        public User LoggedInUser { get; set; }
        public ObservableCollection<ComplexTourRequestCard> Requests { get; set; }
        public List<ComplexTourRequest> ComplexRequests { get; set; }
        private ComplexTourRequestService ComplexTourRequestService;
        private TourRequestService TourRequestService;
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand TutorialCommand { get; set; }

        public TouristComplexTourRequestsWindowViewModel(TouristComplexTourRequestsWindow window, User user)
        {
            parentWindow = window;
            LoggedInUser = user;
            Requests = new ObservableCollection<ComplexTourRequestCard>();
            ComplexRequests = new List<ComplexTourRequest>();
            ComplexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            TourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            CreateCommand = new RelayCommand(CreateClick);
            TutorialCommand = new RelayCommand(TutorialClick);
            Update();
            SetUserControl();
        }

        private void TutorialClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new TutorialStatistics("C:\\Users\\nikol\\Desktop\\Nikola\\Random slike i videi xd\\Random slike i videi xd\\Tutorijali\\ComplexTourRequest.mkv"));
        }

        private void Update()
        {
            ComplexRequests.Clear();
            ComplexRequests = ComplexTourRequestService.GetByTouristId(LoggedInUser.Id);
        }

        private void AreRequestsExpired()
        {
            foreach (var request in ComplexRequests)
            {
                if (request.Status == ComplexTourRequestStatus.Pending_request)
                {
                    if (DateOnly.FromDateTime(DateTime.Today).AddDays(2) >= TourRequestService.GetFurthestDate(request.Id))
                    {
                        request.Status = ComplexTourRequestStatus.Invalid;
                        ComplexTourRequestService.Update(request);
                    }
                }
            }
        }

        private void CheckRequests()
        {
            foreach(var request in ComplexRequests)
            {
                bool should = true;
                if(request.Status == ComplexTourRequestStatus.Pending_request)
                {
                    foreach(TourRequest tourRequest in TourRequestService.GetAllRequestByComplexRequestId(request.Id))
                    {
                        if(tourRequest.Status != TourRequestStatus.Invalid)
                            should = false;
                    }
                    if (should)
                    {
                        request.Status = ComplexTourRequestStatus.Invalid;
                        ComplexTourRequestService.Update(request);
                    }
                }
            }
        }

        private void SetUserControl()
        {
            AreRequestsExpired();
            CheckRequests();
            foreach (ComplexTourRequest request in ComplexRequests)
            {
                if (request.Status != ComplexTourRequestStatus.Invalid)
                {
                    var complexTourRequestCard = new ComplexTourRequestCard(LoggedInUser, request.Id);
                    Requests.Add(complexTourRequestCard);
                }
            }
        }

        private void CreateClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new ComplexTourRequestingWindow(LoggedInUser));
        }
    }
}
