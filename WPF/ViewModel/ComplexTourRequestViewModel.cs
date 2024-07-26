using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModel
{
    public class ComplexTourRequestViewModel: INotifyPropertyChanged
    {
        private int userId;
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
        public RelayCommand HelpCommand { get;}
        public RelayCommand CloseHelpCommand { get;}
        public RelayCommand DemoCommand { get;}
        public RelayCommand StopDemoCommand { get;}
        public RelayCommand LoadRequestsCommand { get;}
        public RelayCommand LoadFreeAppointmentsCommand { get;}
        public RelayCommand AcceptCommand { get;}
        public RelayCommand CloseWindowCommand { get;}
        public ObservableCollection<ComplexTourRequestDTO> ComplexTourRequests { get; set; }
        public ComplexTourRequestDTO SelectedComplexRequest {  get; set; }
        public ComplexTourRequest SavedComplexRequest {  get; set; }
        public ObservableCollection<TourRequestDTO> TourRequests { get; set; }
        public TourRequestDTO SelectedRequest { get; set; }
        public TourRequest SavedRequest { get; set; }
        public ObservableCollection<DateTime> FreeAppointments {  get; set; }
        public DateTime SelectedAppointment { get; set; }
        private ComplexTourRequestService complexTourRequestService;
        private TourRequestService tourRequestService;
        public bool ActivatedDemo { get; set; }
        private ComplexTourRequestWindow window;
        public ComplexTourRequestViewModel(int userId)
        {
            this.userId = userId;
            HelpCommand = new RelayCommand(ShowHelp, CanExecute);
            CloseHelpCommand = new RelayCommand(CloseHelp, CanExecute);
            DemoCommand = new RelayCommand(Demo, CanExecute);
            StopDemoCommand = new RelayCommand(StopDemo, CanExecute);
            LoadRequestsCommand = new RelayCommand(LoadRequests, CanExecute);
            LoadFreeAppointmentsCommand = new RelayCommand(LoadFreeAppointments, CanExecute);
            AcceptCommand = new RelayCommand(AcceptRequest, CanExecute);
            CloseWindowCommand = new RelayCommand(CloseWindow, CanExecute);
            ComplexTourRequests = new ObservableCollection<ComplexTourRequestDTO>();
            SelectedComplexRequest = null;
            SavedComplexRequest = null;
            TourRequests = new ObservableCollection<TourRequestDTO>();
            SelectedRequest = null;
            SavedRequest = null;
            FreeAppointments = new ObservableCollection<DateTime>();
            SelectedAppointment = DateTime.MaxValue;
            complexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()));
            LoadComplexRequests();
        }

        private void LoadComplexRequests()
        {
            ComplexTourRequests.Clear();
            foreach(var complexRequest in complexTourRequestService.GetAll())
            {
                ComplexTourRequests.Add(new ComplexTourRequestDTO(complexRequest));
            }
        }

        private void LoadRequests()
        {
            TourRequests.Clear();
            if(SelectedComplexRequest == null)
            {
                MessageBox.Show("You have not selected complex tour request. Please select complex tour request.");
                return;
            }
            foreach(var request in tourRequestService.GetAllRequestByComplexRequestId(SelectedComplexRequest.Id)) 
            {
                TourRequests.Add(new TourRequestDTO(request));
            }
        }

        private void LoadFreeAppointments()
        {
            FreeAppointments.Clear();
            if(SelectedRequest == null)
            {
                MessageBox.Show("You have not selected tour request. Please select tour request.");
                return;
            }
            foreach(var date in tourRequestService.GetFreeAppointments(SelectedRequest.Id, userId))
            {
                FreeAppointments.Add(date);
            }
        }
        private void AcceptRequest()
        {
            if (ActivatedDemo)
                return;
            if (SelectedAppointment == DateTime.MaxValue)
            {
                MessageBox.Show("You have not selected appointment. Please select appointment.");
                return;
            }
            tourRequestService.Accept(SelectedRequest.ToTourRequest(), userId, SelectedAppointment);
            MessageBox.Show("Congratulations, you have successfully accepted part of the complex tour request!");
            CloseWindow();
        }

        private bool CanExecute()
        {
            return true;
        }
        private void ShowHelp()
        {
            HelpIsOpen = true;
        }
        private void CloseHelp()
        {
            HelpIsOpen = false;
        }
        private void CloseWindow()
        {
            window = (BookingApp.WPF.View.ComplexTourRequestWindow)System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);
            window.Close();
        }
        private async void Demo()
        {
            window = (BookingApp.WPF.View.ComplexTourRequestWindow)System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);
            window.DemoButton.Visibility = Visibility.Hidden;
            window.DemoLabel.Visibility = Visibility.Visible;
            await Task.Delay(1000);
            ActivatedDemo = true;
            SaveApplicationStatus();
            while (ActivatedDemo)
            {

                if (!ActivatedDemo)
                    break;
                SelectComplexRequest();
                if (!ActivatedDemo)
                    break;
                await Task.Delay(1000);
                window.LoadRequestButton.Focus();
                await Task.Delay(1000);
                if (!ActivatedDemo)
                    break;
                LoadRequests();
                if (!ActivatedDemo)
                    break;
                await Task.Delay(1000);
                if (!ActivatedDemo)
                    break;
                SelectRequest();
                if (!ActivatedDemo)
                    break;
                await Task.Delay(1000);
                window.AppointmentButton.Focus();
                await Task.Delay(1000);
                if (!ActivatedDemo)
                    break;
                LoadFreeAppointments();
                if (!ActivatedDemo)
                    break;
                await Task.Delay(1000);
                SelectAppointment();
                if (!ActivatedDemo)
                    break;
                await Task.Delay(1000);
                window.AcceptButton.Focus();
                await Task.Delay(1000);
                TourRequests.Clear();
                FreeAppointments.Clear();
                await Task.Delay(1000);
            }
            window.DemoButton.Visibility = Visibility.Visible;
            window.DemoLabel.Visibility = Visibility.Collapsed;

            GetOldState();
        }

        private void SaveApplicationStatus()
        {
            if (SelectedComplexRequest != null)
                SavedComplexRequest = SelectedComplexRequest.ToComplexTourRequest();
            else
                SavedComplexRequest = null;

            if (SelectedRequest != null)
                SavedRequest = SelectedRequest.ToTourRequest();
            else
                SavedRequest = null;

            TourRequests.Clear();
            FreeAppointments.Clear();

        }
        private void GetOldState()
        {
            if (SavedComplexRequest == null)
                TourRequests.Clear();
            else
            {
                TourRequests.Clear();
                foreach (var request in tourRequestService.GetAllRequestByComplexRequestId(SavedComplexRequest.Id))
                {
                    TourRequests.Add(new TourRequestDTO(request));
                }
            }

            if (SavedRequest == null)
                FreeAppointments.Clear();
            else
            {
                FreeAppointments.Clear();
                foreach (var date in tourRequestService.GetFreeAppointments(SavedRequest.Id, userId))
                {
                    FreeAppointments.Add(date);
                }
            }
            SavedComplexRequest = null;
            SavedRequest = null;
            SelectedComplexRequest = null;
            SelectedRequest = null;
        }

        private void SelectComplexRequest()
        {
            SelectedComplexRequest = ComplexTourRequests.First();
            window.ComplexRequestDataGrid.Focus();
            window.ComplexRequestDataGrid.UpdateLayout();
            window.ComplexRequestDataGrid.ScrollIntoView(SelectedComplexRequest);
            var row = (DataGridRow)window.ComplexRequestDataGrid.ItemContainerGenerator.ContainerFromItem(SelectedComplexRequest);
            if (row != null)
            {
                row.IsSelected = true;
            }
        }
        private void SelectRequest()
        {
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
        private void SelectAppointment()
        {
            SelectedAppointment = FreeAppointments.First();
            window.AppointmentDataGrid.Focus();
            window.AppointmentDataGrid.UpdateLayout();
            window.AppointmentDataGrid.ScrollIntoView(SelectedAppointment);
            var row = (DataGridRow)window.AppointmentDataGrid.ItemContainerGenerator.ContainerFromItem(SelectedAppointment);
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
