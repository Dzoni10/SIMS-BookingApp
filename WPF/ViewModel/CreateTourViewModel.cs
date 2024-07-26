using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.View;
using iText.Svg.Renderers.Path.Impl;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Application.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utilities;
using System.Windows.Input;
using BookingApp.Application.Injector;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.WPF.ViewModel
{
    public class CreateTourViewModel: INotifyPropertyChanged
    {
        private CreateTourService createTourService;
        private TourRequestService tourRequestService;

        public DateTime startDate { get; set; }
        public TourDTO tourDTO { get; set; }
        private string selectedState;
        public string SelectedState 
        {
            get { return selectedState; }
            set
            {
                if (selectedState != value)
                {
                    selectedState = value;
                    OnPropertyChanged("SelectedState");
                    SetCities();
                }
            }
        }
        public string SelectedCity { get; set; }
        public string Language {  get; set; }

        public ObservableCollection<string> ImagesPaths { get; set; }
        public ObservableCollection<DateTime> DateTimes { get; set; }
        public ObservableCollection<string> CheckpointsNames { get; set; }
        public DateTime Date { get; set; }
        public List<string> States { get; set; }

        
        private List<string> cities;
        public List<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        public RelayCommand CreateCommand { get;}
        public RelayCommand CancelCommand { get;}
        public RelayCommand AddCheckpointCommand { get;}
        public RelayCommand AddDateCommand { get;}
        public RelayCommand BrowseButtonCommand { get;}
        private int userId;
        private DateTime date;
        private string checkpointName;

        public CreateTourViewModel(int userId) 
        {
            createTourService = new CreateTourService(new LocationService(Injector.CreateInstance<ILocationRepository>()),
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                new LanguageService(Injector.CreateInstance<ILanguageRepository>()),
                new CheckpointService(Injector.CreateInstance<ICheckpointRepository>()),
                new ImageService(Injector.CreateInstance<IImageRepository>()), 
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))),
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())),
                new NotificationService(Injector.CreateInstance<INotificationRepository>()));
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));

            tourDTO = new TourDTO();
            tourDTO.Capacitys = "0";
            ImagesPaths = new ObservableCollection<string>();
            DateTimes = new ObservableCollection<DateTime>();
            CheckpointsNames = new ObservableCollection<string>();
            States = new List<string>();
            Cities = new List<string>();         
            this.userId = userId;

            CreateCommand = new RelayCommand(Create, CreateCanExecute);
            CancelCommand = new RelayCommand(Cancel, CanExecute);
            AddCheckpointCommand = new RelayCommand(AddCheckpoint, CanExecute);
            AddDateCommand = new RelayCommand(AddDate, CanExecute);
            BrowseButtonCommand = new RelayCommand(AddImage, CanExecute);

            States = createTourService.SetStates();
            SetCities();
            SelectedState = tourRequestService.FindMostWantedState();
            SelectedCity = tourRequestService.FindMostWantedCity(SelectedState);
            Language = tourRequestService.FindMostWantedLanguage();
        }
     
        public bool CreateCanExecute()
        {
            return tourDTO.IsValid && (Language != null && Language != "") && ImagesPaths.Any() && DateTimes.Any() && SelectedCity != null && CheckpointsNames.Count >=2;
        }

        public bool CanExecute()
        {
            return true;
        }
        public void Cancel()
        {
            Messenger.Default.Unregister<NotificationMessage<object>>(this, OnDataReceived);
            Messenger.Default.Unregister<NotificationMessage<object>>(this, OnCheckpointReceived);
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }

        public void Create()
        {
            createTourService.Create(tourDTO.ToTour(), userId, SelectedState, SelectedCity, Language, new List<DateTime>(DateTimes), new List<string>(CheckpointsNames), new List<string>(ImagesPaths));
            MessageBox.Show("Congratulations! You have successfully created a new tour.", "Succesfuly create");
            Messenger.Default.Unregister<NotificationMessage<object>>(this, OnDataReceived);
            Messenger.Default.Unregister<NotificationMessage<object>>(this, OnCheckpointReceived);
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
        private void SetCities()
        {
            Cities = createTourService.SetCities(SelectedState);
        }
        public void AddCheckpoint()
        {
            SelectCheckpointForm selectCheckpointForm = new SelectCheckpointForm();
            Messenger.Default.Register<NotificationMessage<object>>(this, OnCheckpointReceived);
            selectCheckpointForm.ShowDialog();
            CheckpointsNames.Add(checkpointName);
        }
        private void OnCheckpointReceived(NotificationMessage<object> message)
        {
            var data = message.Content;
            checkpointName = Convert.ToString(data);
        }
        public void AddDate()
        {
            SelectDateTimeForm selectDateTimeForm = new SelectDateTimeForm(DateTime.Today.AddDays(-1), DateTime.Today.AddYears(10), false);
            Messenger.Default.Register<NotificationMessage<object>>(this, OnDataReceived);
            selectDateTimeForm.ShowDialog();
            DateTimes.Add(date);
        }
        private void OnDataReceived(NotificationMessage<object> message)
        {
            var data = message.Content;
            date = Convert.ToDateTime(data);
        }
        public void AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Postavite opcije po potrebi
            openFileDialog.Title = "Odaberi datoteku";
            openFileDialog.Filter = "Slike (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp";

            // Prikaži dijalog i provjeri rezultat
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Korisnik je odabrao datoteku, možete dohvatiti putanju
                string selectedFilePath = openFileDialog.FileName;
                ImagesPaths.Add(selectedFilePath);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
