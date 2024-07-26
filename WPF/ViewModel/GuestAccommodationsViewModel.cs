using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel
{
    public class GuestAccommodationsViewModel : INotifyPropertyChanged
    {
        public GuestAccommodations page;
        private readonly GuestWindow parentWindow;
        private readonly User user;

        private AccommodationsService accommodationsService { get; set; }
        private AccommodationSearchService accommodationSearchService { get; set; }
        private LocationService locationService { get; set; }

        public ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public List<AccommodationDTO> FilteredAccommodations { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }
        public RelayCommand ReserveCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand ResetCommand { get; }

        private int numberOfGuests;
        public int NumberOfGuests
        {
            get
            {
                return numberOfGuests;
            }
            set
            {
                if (value != numberOfGuests)
                {
                    numberOfGuests = value;
                    OnPropertyChanged("NumberOfGuests");
                }
            }
        }


        private int daysReserved;
        public int DaysReserved
        {
            get
            {
                return daysReserved;
            }
            set
            {
                if (value != daysReserved)
                {
                    daysReserved = value;
                    OnPropertyChanged("DaysReserved");
                }
            }
        }

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

        public string SelectedCity { get; set; }
        public List<string> States { get; set; }

        public GuestAccommodationsViewModel(GuestWindow parentWindow, User user, GuestAccommodations page)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;
            accommodationsService = new AccommodationsService();
            accommodationSearchService = new AccommodationSearchService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());

            Accommodations = new ObservableCollection<AccommodationDTO>();
            FilteredAccommodations = new List<AccommodationDTO>();

            States = new List<string>();
            Cities = new List<string>();

            ShowAccommodations();
            LoadStatesComboBox();
            LoadCitiesComboBox();
            LoadTypes();

            ReserveCommand = new RelayCommand(ReserveExecute, CanExecute);
            SearchCommand = new RelayCommand(SearchExecute, CanExecute);
            ResetCommand = new RelayCommand(ResetExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }
        public void ReserveExecute()
        {
            if (SelectedAccommodation is null)
            {
                MessageBoxResult result = MessageBox.Show("Morate selektovati vrednost");
                return;
            }
            //parentWindow.MainFrame.NavigationService.Navigate(new ReservationAccommodationForm(this.parentWindow, user, SelectedAccommodation.Id));
            parentWindow.MainFrame.NavigationService.Navigate(new AccommodationPreviewPage(this.parentWindow, user, SelectedAccommodation.Id, new AccommodationReservation(), "create reservation"));
        }
        public void SearchExecute()
        {
            SearchByAccommodationName();
            SearchByState();
            SearchByCity();
            SearchByType();
            SearchByNumberOfGuests();
            SearchByDuration();
            UpdateDataGrid();
        }
        public void ResetExecute()
        {
            page.NameTextBox.Text = string.Empty;
            page.LocationStateComboBox.SelectedItem = null;
            page.LocationCityComboBox.SelectedItem = null;
            States.Clear();
            Cities.Clear();
            LoadStatesComboBox();
            LoadCitiesComboBox();
            page.AccommodationTypeComboBox.SelectedItem = null;
            page.NumberOfGuestsTextBox.Text = "0";
            page.DurationTextBox.Text = "0";
            FilteredAccommodations.Clear();
            ShowAccommodations();
        }
        public void ShowAccommodations()
        {
            foreach(AccommodationDTO accommodationDTO in accommodationsService.GetAccommodationDTOs())
            {
                Accommodations.Add(accommodationDTO);
                FilteredAccommodations.Add(accommodationDTO);
            }
        }
        public void LoadStatesComboBox()
        {
            foreach (Location location in locationService.GetUniqueStates())
            {
                States.Add(location.State);
            }
        }
        public void LoadCitiesComboBox()
        {
            foreach (Location location in locationService.GetUniqueCities())
            {
                Cities.Add(location.City);
            }

        }
        public void SetCities()
        {
            Cities = locationService.GetCitiesForState(SelectedState);
        }
        public void FillLocationCityComboBox(List<Location> locations)
        {
            foreach (Location location in locations)
            {
                page.LocationCityComboBox.Items.Add(location.City);
            }
        }
        public void LoadTypes()
        {
            page.AccommodationTypeComboBox.Items.Add("House");
            page.AccommodationTypeComboBox.Items.Add("Cottage");
            page.AccommodationTypeComboBox.Items.Add("Apartment");
        }
        public void UpdateDataGrid()
        {
            Accommodations.Clear();
            foreach(AccommodationDTO accommodationDTO in FilteredAccommodations)
            {
                Accommodations.Add(accommodationDTO);
            }
            
        }
        public void SearchByAccommodationName()
        {
            if (string.IsNullOrEmpty(page.NameTextBox.Text)) return;
            FilteredAccommodations = accommodationSearchService.FilterAccommodationsByName(FilteredAccommodations, page.NameTextBox.Text);
        }
        public void SearchByState()
        {
            if (page.LocationStateComboBox.SelectedItem is null) return;
            FilteredAccommodations = accommodationSearchService.FilterAccommodationsByState(FilteredAccommodations, page.LocationStateComboBox.SelectedValue.ToString());
        }
        public void SearchByCity()
        {
            if (page.LocationCityComboBox.SelectedItem is null) return;
            FilteredAccommodations = accommodationSearchService.FilterAccommodationsByCity(FilteredAccommodations, page.LocationCityComboBox.SelectedValue.ToString());
        }
        public void SearchByType()
        {
            if (page.AccommodationTypeComboBox.SelectedItem is null) return;
            FilteredAccommodations = accommodationSearchService.FilterAccommodationsByType(FilteredAccommodations, page.AccommodationTypeComboBox.SelectedValue.ToString());
        }
        public void SearchByNumberOfGuests()
        {
            if (string.IsNullOrEmpty(page.NumberOfGuestsTextBox.Text)) return;
            FilteredAccommodations = accommodationSearchService.FilterAccommodationsByNumberOfGuestsDTOs(FilteredAccommodations, page.NumberOfGuestsTextBox.Text);
        }
        public void SearchByDuration()
        {
            if (string.IsNullOrEmpty(page.DurationTextBox.Text)) return;
            FilteredAccommodations = accommodationSearchService.FilterAccommodationsByDuration(FilteredAccommodations, page.DurationTextBox.Text);
            }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
