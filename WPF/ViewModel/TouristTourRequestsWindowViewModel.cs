using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using ceTe.DynamicPDF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel
{
    public class TouristTourRequestsWindowViewModel
    {
        public TouristTourRequestsWindow parentWindow { get; set; }
        public User LoggedInUser { get; set; }
        public ObservableCollection<TourRequestCard> Requests { get; set; }
        public List<TourRequest> TourRequests { get; set; }
        private TourRequestService TourRequestService;
        private TourRequestGuestService TourRequestGuestService;
        public RelayCommand CreateRequestCommand { get; set; }
        public RelayCommand TutorialCommand { get; set; }
        public TouristTourRequestsWindowViewModel(TouristTourRequestsWindow window, User user)
        {
            parentWindow = window;
            LoggedInUser = user;
            Requests = new ObservableCollection<TourRequestCard>();
            TourRequests = new List<TourRequest>();
            TourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            TourRequestGuestService = new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>());
            CreateRequestCommand = new RelayCommand(CreateRequestClick);
            TutorialCommand = new RelayCommand(TutorialClick);
            Update();
            SetRequestsControl();
        }

        private void TutorialClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new TutorialStatistics("C:\\Users\\nikol\\Desktop\\Nikola\\Random slike i videi xd\\Random slike i videi xd\\Tutorijali\\RegularRequestTutorial.mkv"));

        }

        private void Update()
        {
            TourRequests.Clear();
            TourRequests = TourRequestService.GetRequestsByUser(LoggedInUser.Id);
        }

        private void AreRequestsExpired()
        {
            foreach (var request in TourRequests)
            {
                if (request.Status != TourRequestStatus.Accepted)
                {
                    if (DateOnly.FromDateTime(DateTime.Today).AddDays(2) >= request.DateTo)
                    {
                        request.Status = TourRequestStatus.Invalid;
                        TourRequestService.Update(request);
                    }
                }
            }
        }

        private void SetRequestsControl()
        {
            AreRequestsExpired();
            foreach (TourRequest request in TourRequests)
            {
                if (request.Status != TourRequestStatus.Invalid)
                {
                    var tourRequestCard = new TourRequestCard(LoggedInUser);
                    tourRequestCard = LoadLabels(tourRequestCard, request);
                    Requests.Add(tourRequestCard);
                }
            }
        }

        private TourRequestCard LoadLabels(TourRequestCard tourRequestCard, TourRequest tourRequest)
        {
            tourRequestCard.StateLabel.Content = tourRequest.Location.State;
            tourRequestCard.CityLabel.Content = tourRequest.Location.City;
            tourRequestCard.LanguageLabel.Content = tourRequest.Language.Name;
            if (tourRequest.Status == TourRequestStatus.Pending_request)
                tourRequestCard.StatusLabel.Content = "Pending request";
            else
                tourRequestCard.StatusLabel.Content = tourRequest.Status.ToString();
            tourRequestCard.DescriptionTextBox.Text = tourRequest.Description;
            if(tourRequest.Status == TourRequestStatus.Accepted)
            {
                tourRequestCard.DateFromLabel.Visibility = Visibility.Collapsed;
                tourRequestCard.DateToLabel.Visibility = Visibility.Collapsed;
                tourRequestCard.AcceptedLabel.Visibility = Visibility.Visible;
                tourRequestCard.AcceptedLabel.Content += tourRequest.DateIfAccepted.ToString("dd.MM.yyyy HH:mm");
            }
            tourRequestCard.DateFromLabel.Content += tourRequest.DateFrom.ToString("dd.MM.yyyy");
            tourRequestCard.DateToLabel.Content += tourRequest.DateTo.ToString("dd.MM.yyyy");
            tourRequestCard = GetTourGuests(tourRequestCard, tourRequest);
            return tourRequestCard;
        }

        private void CreateRequestClick()
        {
            CollapseGridChildrenExceptMainFrame(parentWindow.ThisGrid);
            parentWindow.MainFrame.Visibility = Visibility.Visible;
            parentWindow.MainFrame.Content = new CreateSimpleTourRequest(LoggedInUser);
        }

        private void CollapseGridChildrenExceptMainFrame(Grid grid)
        {
            foreach (UIElement child in grid.Children)
            {
                // Preskače MainFrame
                if (child == parentWindow.MainFrame)
                    continue;

                // Postavlja Visibility na Collapsed za sve osim MainFrame-a
                child.Visibility = Visibility.Collapsed;

                // Ako je dijete Grid, rekurzivno poziva ovu funkciju
                if (child is Grid)
                {
                    CollapseGridChildrenExceptMainFrame(child as Grid);
                }
            }
        }

        private TourRequestCard GetTourGuests(TourRequestCard tourRequestCard, TourRequest tourRequest)
        {
            List<TourRequestGuest> guests = TourRequestGuestService.GetByRequestId(tourRequest.Id);
            foreach (TourRequestGuest guest in guests)
            {
                tourRequestCard.TourGuests.Add(new TourRequestGuestDTO(guest));
            }
            return tourRequestCard;
        }
    }
}
