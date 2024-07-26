using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Utilities;
using System.Windows.Input;
using System.Data.Common;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace BookingApp.WPF.ViewModel
{
    public class GuideViewModel : INotifyPropertyChanged
    {
        private TourService tourService;
        private StartTourDateService startTourDateService;
        private ResignService resignService;
        private GuideService guideService;
        public TourDTO SelectedFinishedTour { get; set; }
        public string GuideName { get; set;}
        private User user;

        public ObservableCollection<TourDTO> FinishedTours { get; set; }
        public ObservableCollection<TourDTO> MostVisitedTour { get; set; }
        public List<int> ComboboxYears { get; set; }

        public RelayCommand FollowTourCommand { get;}
        public RelayCommand CreateTourCommand { get;}
        public RelayCommand TourStatisticsCommand { get;}
        public RelayCommand CancelTourCommand { get;}
        public RelayCommand ReviewCommand { get;}
        public RelayCommand TourRequestCommand { get;}
        public RelayCommand ComplexTourRequestCommand { get;}
        public RelayCommand TourRequestStatisticCommand { get;}
        public RelayCommand ResignCommand { get;}
        public RelayCommand HelpCommand { get; }
        public RelayCommand CloseHelpCommand { get; }
        public RelayCommand CloseWindowCommand { get; }
        public event EventHandler RequestClose;
        private bool helpIsOpen;
        public bool HelpIsOpen
        {
            get { return helpIsOpen; }
            set
            {
                if (helpIsOpen != value)
                {
                    helpIsOpen = value;
                    OnPropertyChanged(nameof(HelpIsOpen));
                }
            }
        }

        private Visibility superGuideVisibility;
        public Visibility SuperGuideVisibility
        {
            get { return superGuideVisibility; }
            set
            {
                if (superGuideVisibility != value)
                {
                    superGuideVisibility = value;
                    OnPropertyChanged(nameof(SuperGuideVisibility));
                }
            }
        }
        public bool SuperGuide {  get; set; }
        public GuideViewModel(User user)
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            startTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());         
            resignService = new ResignService(new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                            new VoucherService(Injector.CreateInstance<IVoucherRepository>(), new UserService(), startTourDateService,
                            new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), tourService)),
                            new TourService(Injector.CreateInstance<ITourRepository>(),
                            new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                            new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))),
                            new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), tourService),
                            new NotificationService(Injector.CreateInstance<INotificationRepository>()),
                            new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            guideService = new GuideService(Injector.CreateInstance<IGuideRepository>(), new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                new TourRatingService(Injector.CreateInstance<ITourRatingRepository>()),
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))));
            GuideName = user.Username;
            this.user = user;
            HelpIsOpen = false;

            FollowTourCommand = new RelayCommand(FollowTour, CanExecute);
            CreateTourCommand = new RelayCommand(CreateTour, CanExecute);
            TourStatisticsCommand = new RelayCommand(TourStatistic, CanExecute);
            CancelTourCommand = new RelayCommand(CancelTour, CanExecute);
            ReviewCommand = new RelayCommand(Review, CanExecute);
            TourRequestCommand = new RelayCommand(AcceptTourRequest, CanExecute);
            ComplexTourRequestCommand = new RelayCommand(AcceptPartOfComplexTourRequest, CanExecute);
            TourRequestStatisticCommand = new RelayCommand(TourRequestStatistic, CanExecute);
            HelpCommand = new RelayCommand(Help, CanExecute);
            CloseHelpCommand = new RelayCommand(CloseHelp, CanExecute);
            CloseWindowCommand = new RelayCommand(CloseWindow, CanExecute);
            ResignCommand = new RelayCommand(Resign, CanExecute);

            FinishedTours = new ObservableCollection<TourDTO>();
            MostVisitedTour = new ObservableCollection<TourDTO>();
            ComboboxYears = new List<int>();
            UpdateFinishedTour();
            SetComboBoxOptions();
            FindMostVisitedTour();
            guideService.SetSuperGuide(user.Id);
            SuperGuide = guideService.GetByGuideId(user.Id).IsSuperGuide;
            SetStarVisibility();
        }
        private void SetStarVisibility()
        {
            if (SuperGuide)
            {
                SuperGuideVisibility = Visibility.Visible;
            }
            else
            {
                SuperGuideVisibility = Visibility.Collapsed;
            }
        }
        private bool CanExecute()
        {
            return true;
        }
        public void UpdateFinishedTour()
        {
            FinishedTours.Clear();

            foreach (StartTourDate startDate in startTourDateService.GetFinishedTours())
            {
                AddTours(startDate);
            }
        }

        public void FindMostVisitedTour()
        {
            int max = 0;
            foreach (TourDTO tourDTO in FinishedTours)
            {
                if (tourDTO.Visitors > max)
                {
                    max = tourDTO.Visitors;
                    MostVisitedTour.Clear();
                    MostVisitedTour.Add(tourDTO);
                }
            }
        }

        private void AddTours(StartTourDate startDate)
        {
            foreach (Tour tour in tourService.GetToursByUserAndTourId(user.Id, startDate.TourId))
            {
                TourDTO tourDTO = new TourDTO(tour);
                tourDTO.StartTourDate = startDate;
                tourDTO.Visitors = tourDTO.Capacity - startDate.Capacity;
                FinishedTours.Add(tourDTO);
            }
        }

        public void SetComboBoxOptions()
        {
            ComboboxYears = startTourDateService.GetYears();
        }

        public void CreateTour()
        {
            CreateTourForm createTourForm = new CreateTourForm(user.Id);
            createTourForm.ShowDialog();
        }

        public void FollowTour()
        {
            FollowTourWindow followTourWindow = new FollowTourWindow(user.Id);
            followTourWindow.ShowDialog();
        }

        public void TourStatistic()
        {
            if (SelectedFinishedTour == null)
            {
                MessageBox.Show("You have NOT selected a tour. Please select a tour to get the statistics.");
                return;
            }
          
            TourStatisitcsWindow tourStatisitcsWindow = new TourStatisitcsWindow(SelectedFinishedTour);
            tourStatisitcsWindow.ShowDialog();
            
        }

        public void ComboBoxSelectionChanged(int sledectedYear)
        {

            int year = sledectedYear;
            int max = 0;
            foreach (TourDTO tourDTO in FinishedTours.Where(tour => tour.StartTourDate.Date.Year == year).ToList())
            {
                if (tourDTO.Visitors > max)
                {
                    max = tourDTO.Visitors;
                    MostVisitedTour.Clear();
                    MostVisitedTour.Add(tourDTO);
                }
            }
        }

        public void CancelTour()
        {
            CancelTourWindow cancelTourWindow = new CancelTourWindow(user.Id);
            cancelTourWindow.ShowDialog();
        }

        public void Review()
        {
            ReviewWindow reviewWindow = new ReviewWindow(user.Id);
            reviewWindow.ShowDialog();
        }

        private void AcceptTourRequest()
        {
            TourRequestWindow tourRequestWindow = new TourRequestWindow(user.Id);
            tourRequestWindow.ShowDialog();
        }
        private void AcceptPartOfComplexTourRequest()
        {
            ComplexTourRequestWindow complexTourRequestWindow = new ComplexTourRequestWindow(user.Id);
            complexTourRequestWindow.ShowDialog();
        }
        private void TourRequestStatistic()
        {
            TourRequestStatisticsWindow tourRequestStatisticsWindow = new TourRequestStatisticsWindow();
            tourRequestStatisticsWindow.ShowDialog();
        }

        private void Resign()
        {
            MessageBoxResult result =  MessageBox.Show("Are you sure you want to resign. If you resign, all your tours will be cancelled.","Resign", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                resignService.Resign(user.Id);
                MessageBox.Show("You resigned. All your tours have been cancelled.");
            }
        }
        private void Help()
        {
            HelpIsOpen = true;
        }
        private void CloseHelp()
        {
            HelpIsOpen = false;
        }
        private void CloseWindow()
        {
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
