using BookingApp.Domain.Models;
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
using System.Runtime.CompilerServices;
using BookingApp.Utilities;
using iText.Forms.Form.Element;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.WPF.ViewModel
{
    public class TouristWindowViewModel
    {
        private readonly TouristWindow parentWindow;
        public User LoggedInUser { get; set; }
        public ObservableCollection<TourDTO> Toures { get; set; }
        public ObservableCollection<CheckpointDTO> Checkpoints { get; set; }
        private StartTourDateService StartTourDateService;

        public RelayCommand ToursMenuCommand { get; set; }
        public RelayCommand ShowToursCommand { get; set;}
        public RelayCommand OngoingTourCommand { get; set; }
        public RelayCommand RateTourCommand { get; set; }
        public RelayCommand RequestMenuCommand { get; set; }
        public RelayCommand VouchersMenuCommand { get; set; }
        public RelayCommand LogOutMenuCommand { get; set; }
        public RelayCommand NotificationMenuCommand { get; set; }
        public RelayCommand TourRequestsStatsCommand { get; set; }
        public RelayCommand RegularTourCommand { get; set; }
        public RelayCommand ComplexTourCommand { get; set; }

        public TouristWindowViewModel(TouristWindow touristWindow, User user)
        {
            parentWindow = touristWindow;
            LoggedInUser = user;
            parentWindow.Main.Navigate(new ShowTours(LoggedInUser));
            StartTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            Toures = new ObservableCollection<TourDTO>();
            Checkpoints = new ObservableCollection<CheckpointDTO>();

            ToursMenuCommand = new RelayCommand(ToursMenuClick, CanExecute);
            ShowToursCommand = new RelayCommand(ShowToursClick, CanExecute);
            OngoingTourCommand = new RelayCommand(OngoingTourClick, CanExecute);
            RateTourCommand = new RelayCommand(RateToursClick, CanExecute);
            RequestMenuCommand = new RelayCommand(RequestMenuClick);
            VouchersMenuCommand = new RelayCommand(VoucherMenuClick, CanExecute);
            LogOutMenuCommand = new RelayCommand(LogOutMenuClick, CanExecute);
            NotificationMenuCommand = new RelayCommand(NotificationMenuClick, CanExecute);
            TourRequestsStatsCommand = new RelayCommand(TourRequestsStatsClick);
            RegularTourCommand = new RelayCommand(RegularTourClick);
            ComplexTourCommand = new RelayCommand(ComplexTourClick);
        }

        public bool CanExecute()
        {
            return true;
        }

        private void RequestMenuClick()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem optionRegularTours = new MenuItem { Header = "Regular tour" };
            optionRegularTours.Command = RegularTourCommand;
            MenuItem optionComplexTours = new MenuItem { Header = "Complex tour" };
            optionComplexTours.Command = ComplexTourCommand;

            contextMenu.Items.Add(optionRegularTours);
            contextMenu.Items.Add(optionComplexTours);

            // Prikazati meni
            contextMenu.FontSize = 30;
            contextMenu.IsOpen = true;
            //parentWindow.Main.Content = new ComplexTourRequestingWindow(parentWindow.LoggedInUser);
        }

        private void RegularTourClick()
        {
            parentWindow.Main.Content = new TouristTourRequestsWindow(LoggedInUser);
        }

        private void ComplexTourClick()
        {
            parentWindow.Main.Content = new TouristComplexTourRequestsWindow(LoggedInUser);
        }

        private void TourRequestsStatsClick()
        {
            parentWindow.Main.Content = new TouristTourRequestStatsWindow(LoggedInUser);
        }

        private void ToursMenuClick()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem optionShowTours = new MenuItem { Header = "Show tours" };
            optionShowTours.Command = ShowToursCommand; 
            MenuItem optionOngoingTour = new MenuItem { Header = "Your active tours" };
            optionOngoingTour.Command = OngoingTourCommand;
            MenuItem optionRateTours = new MenuItem { Header = "Rate tours" };
            optionRateTours.Command = RateTourCommand;

            contextMenu.Items.Add(optionShowTours);
            contextMenu.Items.Add(optionOngoingTour);
            contextMenu.Items.Add(optionRateTours);

            // Prikazati meni
            contextMenu.FontSize = 30;
            contextMenu.IsOpen = true;
        }

        private void NotificationMenuClick()
        {
            parentWindow.Main.Content = new TouristNotificationWindow(LoggedInUser);
        }

        private void LogOutMenuClick()
        {
            parentWindow.Close();
        }

        private void VoucherMenuClick()
        {
            parentWindow.Main.Content = new ShowVouchersWindow(LoggedInUser);
        }

        private void RateToursClick()
        {
            parentWindow.Main.Content = new RateTourAndGuide(LoggedInUser);
        }

        private void ShowToursClick()
        {
            parentWindow.Main.Content = new ShowTours(LoggedInUser);
        }

        private void OngoingTourClick()
        {
            StartTourDate? tour = StartTourDateService.GetActiveTour();
            if(tour != null) 
                parentWindow.Main.Content = new OngoingTour(LoggedInUser);
        }

        
    }
}
