using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel
{
    public class ComplexTourRequestCardViewModel
    {
        public ComplexTourRequestCard parentWindow { get; set; }
        public User LoggedInUser { get; set; }
        public int ComplexTourRequestId { get; set; }
        public ObservableCollection<TourRequestDTO> Requests { get; set; }
        private ComplexTourRequestService ComplexTourRequestService;
        private TourRequestService TourRequestService;

        public ComplexTourRequestCardViewModel(ComplexTourRequestCard window, User user)
        {
            parentWindow = window;
            ComplexTourRequestId = parentWindow.ComplexTourRequestId;
            LoggedInUser = user;
            Requests = new ObservableCollection<TourRequestDTO>();
            ComplexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            TourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            Update();
            UpdateLabel();
        }

        private void Update()
        {
            List<TourRequest> requests = TourRequestService.GetAll().Where(r => r.ComplexTourRequestId == ComplexTourRequestId).ToList();
            foreach (TourRequest request in requests)
            {
                Requests.Add(new TourRequestDTO(request));
            }
        }

        private void UpdateLabel()
        {
            ComplexTourRequest request = ComplexTourRequestService.GetById(ComplexTourRequestId);
            if (request.Status == ComplexTourRequestStatus.Pending_request)
                parentWindow.StatusLabel.Content += "Pending request";
            else
                parentWindow.StatusLabel.Content += request.Status.ToString();
        }
    }
}
