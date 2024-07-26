using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utilities;

namespace BookingApp.WPF.ViewModel
{
    public class TourStatisitcsViewModel
    {
        public TourDTO SelectedTour { get; set; }
        public int Under18 { get; set; }
        public int Between18_50 { get; set; }
        public int Over50 { get; set; }
        private TourStatisticsService tourStatisticsService;
        public RelayCommand OKCommand { get; }
        public event EventHandler RequestClose;
        public TourStatisitcsViewModel(TourDTO selectedTour)
        {
            tourStatisticsService = new TourStatisticsService(new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), 
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())))),
                new TourGuestService(Injector.CreateInstance<ITourGuestRepository>()));
            SelectedTour = selectedTour;
            OKCommand = new RelayCommand(Close);
            Under18 = tourStatisticsService.CalculateUnder18(SelectedTour.StartTourDate.Id);
            Between18_50 = tourStatisticsService.CalculateBetween18_50(SelectedTour.StartTourDate.Id);
            Over50 = tourStatisticsService.CalculateOver50(SelectedTour.StartTourDate.Id);
        }
        private void Close()
        {
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
    }
}
