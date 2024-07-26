using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using ceTe.DynamicPDF.Forms;
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
    class AccommodationForumsViewModel : INotifyPropertyChanged
    {
        public AccommodationForums page;
        private readonly GuestWindow parentWindow;
        private readonly User user;
        private LocationService locationService { get; set; }
        private ForumService forumService { get; set; }

        public ObservableCollection<LocationDTO> Locations { get; set; }
        public List<LocationDTO> FilteredLocations { get; set; }
        public LocationDTO SelectedLocation { get; set; }
        public ForumDTO Forum { get; set; }
        public RelayCommand CreateForumCommand { get; }
        public RelayCommand OpenForumCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand ResetCommand { get; }

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

        public AccommodationForumsViewModel(GuestWindow parentWindow, User user, AccommodationForums page)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>());

            Locations = new ObservableCollection<LocationDTO>();
            Forum = new ForumDTO();
            FilteredLocations = new List<LocationDTO>(Locations);

            States = new List<string>();
            Cities = new List<string>();

            ShowLocations();
            LoadStatesComboBox();
            LoadCitiesComboBox();

            CreateForumCommand = new RelayCommand(CreateForumExecute, CanExecute);
            OpenForumCommand = new RelayCommand(OpenForumExecute, CanExecute);
            SearchCommand = new RelayCommand(SearchExecute, CanExecute);
            ResetCommand = new RelayCommand(ResetExecute, CanExecute);
        }
        public bool CanExecute()
        {
            return true;
        }
        public void CreateForumExecute()
        {
            if (SelectedLocation is null)
            {
                return;
            }
            parentWindow.MainFrame.NavigationService.Navigate(new AccommodationForumPage(this.parentWindow, user, SelectedLocation.Id, "create"));
        }
        public void OpenForumExecute()
        {
            if (SelectedLocation is null)
            {
                return;
            }
            parentWindow.MainFrame.NavigationService.Navigate(new AccommodationForumPage(this.parentWindow, user, SelectedLocation.Id, "open"));
        }
        
        public void SearchExecute()
        {
            SearchByState();
            SearchByCity();
        }
    
        public void ResetExecute()
        {

            page.LocationStateComboBox.SelectedItem = null;
            page.LocationCityComboBox.SelectedItem = null;
            States.Clear();
            Cities.Clear();
            LoadStatesComboBox();
            LoadCitiesComboBox();
            Locations.Clear();
            ShowLocations();
        }

        public void ShowLocations()
        {            
            List<Location> locations = locationService.GetAll();
            foreach(Location location in locations) {
                if(forumService.ExistsForLocation(location.Id))
                {
                    LocationDTO locationDTO = new LocationDTO(location.Id, location.State, location.City, !forumService.ExistsForLocation(location.Id), forumService.IsVeryUseful(forumService.GetIdFromLocationId(location.Id)));
                    //locationDTO.VeryUsefulForum = forumService.IsVeryUseful(forumService.GetIdFromLocationId(location.Id));
                    Locations.Add(locationDTO);
                }
                else
                {
                    Locations.Add(new LocationDTO(location.Id, location.State, location.City, !forumService.ExistsForLocation(location.Id), false));
                }
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
        
        public void SearchByState()
        {
            if (page.LocationStateComboBox.SelectedItem is null) return;
            List<Location> locations = locationService.GetLocationsByState(page.LocationStateComboBox.SelectedValue.ToString());
            Locations.Clear();
            foreach(Location location in locations)
            {
                Locations.Add(new LocationDTO(location));
            }
        }
        public void SearchByCity()
        {
            if (page.LocationCityComboBox.SelectedItem is null) return;
            List<Location> locations = locationService.GetLocationsByCity(page.LocationCityComboBox.SelectedValue.ToString());
            Locations.Clear();
            foreach(Location location in locations)
            {
                Locations.Add(new LocationDTO(location));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
