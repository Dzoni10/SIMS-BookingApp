using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace BookingApp.WPF.ViewModel
{
    public class TourRequestViewModel : INotifyPropertyChanged
    {
        private string selectedState;
        public string SelectedState
        {
            get { return selectedState; }
            set
            {
                if (selectedState != value)
                {
                    selectedState = value;
                    OnPropertyChanged(nameof(SelectedState));
                    SetCities();
                }
            }
        }
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }
        private string capacity;
        public string Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");              
                }
            }
        }
        private string language;
        public string Language 
        {
            get { return language; }
            set 
            {
                if(value != language)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        public List<string> States { get; set; }
        private List<string> _cities;
        public List<string> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }
        public ObservableCollection<TourRequestDTO> TourRequests { get; set; }

        private TourRequestService tourRequestService;
        private SearchTourRequestService searchTourRequestService;
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public TourRequestDTO SelectedRequest { get; set; }
        private DateTime arrangedAppointment;
        private TourRequestWindow window;
        private int guideId;
        private string dateLabel;
        public string DateLabel
        {
            get { return dateLabel;  }
            set
            {
                if(value != dateLabel)
                {
                    dateLabel = value;
                    OnPropertyChanged(nameof(DateLabel));
                }
            }
        }
        private List<TourRequest> searchRequests;
        public RelayCommand SelectDateCommand { get;}
        public RelayCommand AcceptCommand { get;}
        public RelayCommand SearchCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand HelpCommand { get; }
        public RelayCommand CloseHelpCommand { get; }
        public RelayCommand DemoCommand { get; }
        public RelayCommand StopDemoCommand { get; }
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
        public bool ActivatedDemo { get; set; }
        private TourRequestDTO savedRequest;
        private DateTime savedDate;

        public TourRequestViewModel(int guideId)
        {       
            TourRequests = new ObservableCollection<TourRequestDTO>();
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            searchTourRequestService = new SearchTourRequestService(new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())),
                new LocationService(Injector.CreateInstance<ILocationRepository>()),
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))),
                new NotificationService(Injector.CreateInstance<INotificationRepository>()));
            SearchCommand = new RelayCommand(Search, FilterCanExecute);
            ResetCommand = new RelayCommand(ResetFilter, CanExecute);
            SelectDateCommand = new RelayCommand(SelectDate, CanExecute);
            AcceptCommand = new RelayCommand(Accept, CanExecute);
            CancelCommand = new RelayCommand(Cancel, CanExecute);
            HelpCommand = new RelayCommand(Help, CanExecute);
            CloseHelpCommand = new RelayCommand(CloseHelp, CanExecute);
            DemoCommand = new RelayCommand(Demo, CanExecute);
            StopDemoCommand = new RelayCommand(StopDemo, CanExecute);
            searchRequests = new List<TourRequest>();
            arrangedAppointment = DateTime.MinValue;
            SetBlackoutDates();
            this.guideId = guideId;

            EndDate = DateTime.Now.AddYears(10);
            DateLabel = "";
            Capacity = "0";
            SetStates();
            SetCities();
            UpdateRequests();
        }
        private async void SetBlackoutDates()
        {
            await Task.Delay(1000);
            this.window = (BookingApp.WPF.View.TourRequestWindow)System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);
            window.DateFrom.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
            window.DateTo.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.MaxValue));
        }
        private bool FilterCanExecute()
        {
            int r;
            if (!int.TryParse(Capacity, out r))
            {
                return false;
            }
            return true;
        }
        private bool CanExecute()
        {
            return true;
        }
        private void UpdateRequests()
        {
            TourRequests.Clear();
            searchRequests = tourRequestService.GetPendingRequests();
            foreach (var request in searchRequests)
            {
                TourRequests.Add(new TourRequestDTO(request));
            }
        }

        private void Search()
        {
            SearchByLocation();
            SearchByCapacity();
            SearchByLanguage();
            SearchByDate();
            TourRequests.Clear();
            foreach (var request in searchRequests)
            {
                TourRequests.Add(new TourRequestDTO(request));
            }
        }
        private void ResetFilter()
        {
            SelectedCity = null;
            SelectedState = null;
            Capacity = "0";
            Language = "";
            window.DateFrom.SelectedDate = null;
            window.DateTo.SelectedDate = null;
            window.DateTo.BlackoutDates.Clear();
            window.DateFrom.BlackoutDates.Clear();
            window.DateFrom.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
            window.DateTo.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.MaxValue));
            EndDate = DateTime.Now.AddYears(10);
            UpdateRequests();
        }

        private void SetStates()
        {
            States = searchTourRequestService.SetStates();
        }
        private void SetCities()
        {
            Cities = searchTourRequestService.SetCities(SelectedState);
        }

        private void SearchByCapacity()
        {
            searchRequests = searchTourRequestService.SearchByCapacity(searchRequests, Convert.ToInt32(Capacity));
            
        }
        
        private void SearchByLocation()
        {
            searchRequests = searchTourRequestService.SearchByLocation(searchRequests, SelectedState, SelectedCity);
        }

        private void SearchByLanguage()
        {
            searchRequests = searchTourRequestService.SearchByLanguage(searchRequests, Language);
        }

        public void SearchByDate()
        {
            searchRequests = searchTourRequestService.SearchByDate(searchRequests, StartDate, EndDate);
        }

        public void StartDateChanged(DateTime? startDate)
        {
            if (startDate.HasValue)
            {
                StartDate = startDate.Value;
            }
            window.DateTo.BlackoutDates.Clear();
            window.DateTo.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, StartDate));
        }
        public void EndDateChanged(DateTime? endDate)
        {
            if (endDate.HasValue)
            {
                EndDate = endDate.Value;
            }
            window.DateFrom.BlackoutDates.Clear();
            window.DateFrom.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
            window.DateFrom.BlackoutDates.Add(new CalendarDateRange(EndDate, DateTime.Now.AddYears(100)));
        }
        private void SelectDate()
        {
            if (SelectedRequest == null && !ActivatedDemo)
            {
                MessageBox.Show("Before selecting a date, you have to select a request. Please select request.");
                return;
            }
            DateTime timeFrom = (SelectedRequest.DateFrom.ToDateTime(TimeOnly.MinValue) > DateTime.Now) ? SelectedRequest.DateFrom.ToDateTime(TimeOnly.MinValue) : DateTime.Now;
            SelectDateTimeForm selectDateTimeForm = new SelectDateTimeForm(timeFrom, SelectedRequest.DateTo.ToDateTime(TimeOnly.MinValue), ActivatedDemo);
            Messenger.Default.Register<NotificationMessage<object>>(this, OnDataReceived);
            selectDateTimeForm.ShowDialog();
            if(arrangedAppointment == DateTime.MinValue)
            {
                ActivatedDemo = false;
                return;
            }
            DateLabel = arrangedAppointment.ToString("dd.MM.yyyy HH:mm");
            window.Calendar.Visibility = Visibility.Visible;
            window.WhenCalendar.Visibility = Visibility.Hidden;
        }
        private void OnDataReceived(NotificationMessage<object> message)
        {
            var data = message.Content;
            arrangedAppointment = Convert.ToDateTime(data);
        }

        private void Accept()
        {
            if (ActivatedDemo)
                return;
            if(arrangedAppointment == DateTime.MinValue)
            {
                MessageBox.Show("You cannot allow the request because you have not selected a date!");
                return;
            }
            if(!searchTourRequestService.IsDateValid(arrangedAppointment, guideId))
            {
                MessageBox.Show("You have a scheduled tour in the selected time slot, please choose a new time slot.");
                arrangedAppointment = DateTime.MinValue;
                DateLabel = "";
                window.Calendar.Visibility = Visibility.Hidden;
                window.WhenCalendar.Visibility = Visibility.Visible;
                return;
            }
            searchTourRequestService.Accept(SelectedRequest.ToTourRequest(), guideId, arrangedAppointment);           
            MessageBox.Show("Congradulation! You have accepted tour request:\n"+SelectedRequest.Location.State+", "+SelectedRequest.Location.City+", "+SelectedRequest.Language.Name+", "+arrangedAppointment.ToString("dd.MM.yyyy HH:mm"), "Accepted");
            TourRequests.Remove(SelectedRequest);
            Messenger.Default.Unregister<NotificationMessage<object>>(this, OnDataReceived);
            window.Close();           
        }
        private void Cancel()
        {
            if (ActivatedDemo)
                return;
            Messenger.Default.Unregister<NotificationMessage<object>>(this, OnDataReceived);
            window.Close();
        }
        private void Help()
        {
            HelpIsOpen = true;
        }
        private void CloseHelp()
        {
            HelpIsOpen = false;
        }

        private async void Demo()
        {
            SetBlackoutDates();
            if (TourRequests.Count == 0)
                return;
            if (SelectedRequest == null)
                savedRequest = null;
            else
                savedRequest = SelectedRequest;
            savedDate = arrangedAppointment;
            DateLabel = "";
            window.Calendar.Visibility = Visibility.Hidden;
            window.WhenCalendar.Visibility = Visibility.Visible;
            //SelectedRequest = TourRequests.First();
            window.DemoButton.Visibility = Visibility.Hidden;
            window.DemoLabel.Visibility = Visibility.Visible;
            ActivatedDemo = true;
            while (ActivatedDemo)
            {
                await Task.Delay(1000);
                if (!ActivatedDemo)
                {
                    break;
                }
                SelectItem();
                if (!ActivatedDemo)
                {
                    break;
                }
                await Task.Delay(1000);
                window.AddDate.Focus();
                await Task.Delay(1000);
                if (!ActivatedDemo)
                { 
                    break;
                }
                SelectDate();
                if (!ActivatedDemo)
                {
                    break;
                }
                await Task.Delay(1000);
                window.AcceptButton.Focus();
                if (!ActivatedDemo)
                {
                    break;
                }
                await Task.Delay(1000);

                DateLabel = "";
                window.Calendar.Visibility = Visibility.Hidden;
                window.WhenCalendar.Visibility = Visibility.Visible;
            }
            window.DemoButton.Visibility = Visibility.Visible;
            window.DemoLabel.Visibility = Visibility.Hidden;
            DateLabel = "";
            window.Calendar.Visibility = Visibility.Hidden;
            window.WhenCalendar.Visibility = Visibility.Visible;
            GetInitialStata();
        }
        
        public void GetInitialStata()
        {
            arrangedAppointment = DateTime.MinValue;
            if(savedDate == DateTime.MinValue)
            {
                DateLabel = "";
                window.Calendar.Visibility = Visibility.Hidden;
                window.WhenCalendar.Visibility = Visibility.Visible;
            }
            else
            {
                arrangedAppointment = savedDate;
                DateLabel = arrangedAppointment.ToString("dd.MM.yyyy HH:mm");
                window.Calendar.Visibility = Visibility.Visible;
                window.WhenCalendar.Visibility = Visibility.Hidden;
            }
            SelectedRequest = savedRequest;
            //SelectItem(SelectedRequest);
        }
        private void SelectItem() 
        {
            if(SelectedRequest == null)
                SelectedRequest = TourRequests.First();
            window.RequestDataGrid.Focus();
            window.RequestDataGrid.UpdateLayout();
            window.RequestDataGrid.ScrollIntoView(SelectedRequest);
            var row = (DataGridRow)window.RequestDataGrid.ItemContainerGenerator.ContainerFromItem(SelectedRequest);
            if (row != null)
            {
                row.IsSelected = true;
            }
        }
        private void StopDemo()
        {
            ActivatedDemo = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
