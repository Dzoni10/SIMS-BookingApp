using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel
{
    public class CancelTourViewModel
    {
        private CancelTourService cancelTourService;
        private TourService tourService;
        private StartTourDateService startTourDateService;

        private int userId;
        public event EventHandler RequestClose;

        public ObservableCollection<TourDTO> FutureTours { get; set; }
        public TourDTO SelectedFutureTour { get; set; }

        public RelayCommand QuitTourCommand { get;}
        public RelayCommand OkCommand { get;}
        public CancelTourViewModel(int userId)
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            startTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            cancelTourService = new CancelTourService(new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), tourService),
                new VoucherService(Injector.CreateInstance<IVoucherRepository>(), new UserService(), startTourDateService, 
                new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), tourService)),
                new NotificationService(Injector.CreateInstance<INotificationRepository>()));
            this.userId = userId;

            QuitTourCommand = new RelayCommand(QuitTour, CanExecute);
            OkCommand = new RelayCommand(Ok, CanExecute);

            FutureTours = new ObservableCollection<TourDTO>();
            SelectedFutureTour = new TourDTO();
            UpdateFinishedTour();
        }
        private bool CanExecute()
        {
            return true;
        }

        public void UpdateFinishedTour()
        {
            FutureTours.Clear();
            foreach(StartTourDate startDate in startTourDateService.GetToursToCancel())
            {
                AddTours(startDate);
            }
        }

        private void AddTours(StartTourDate startDate)
        {
            foreach(Tour tour in tourService.GetToursByUserAndTourId(userId, startDate.TourId))
            {
                TourDTO tourDTO = new TourDTO(tour);
                tourDTO.StartTourDate = startDate;
                FutureTours.Add(tourDTO);
            }
        }

        public void QuitTour()
        {
            if(FutureTours.Count == 0)
            {
                MessageBox.Show("There is not tour.");
                return;
            }
            if (SelectedFutureTour == null)
            {
                MessageBox.Show("You have to select tour.");
                return;
            }
            
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cacnel this tour?", "Cancel Tour", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                cancelTourService.Canacel(userId, SelectedFutureTour.StartTourDate.Id);
                FutureTours.Remove(SelectedFutureTour);
            }
                
            
        }
        private void Ok()
        {
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
    }
}
