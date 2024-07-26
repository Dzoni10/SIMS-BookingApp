using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using GalaSoft.MvvmLight.Messaging;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace BookingApp.WPF.ViewModel
{
    public class CheckTouristViewModel
    {
        private TourGuestService tourGuestService;
        private TourReservationService tourReservationService;

        public ObservableCollection<TourGuestDTO> tourGuests { get; set; }
        public TourGuestDTO slectedTourGuest { get; set; }
        private int startTourDateId;
        private int checkpointId;
        public RelayCommand CancelCommand { get; }
        public RelayCommand CheckTouristCommand { get; }
        public RelayCommand StopDemoCommand { get; }
        public RelayCommand OkCommand { get; }
        public RelayCommand CheckBoxCheckedCommand { get; }
        public RelayCommand CheckBoxUncheckedCommand { get; }
        private CheckTouristForm window;
        private bool stopDemo;
        public bool ActivatedDemo { get; set; }
        public CheckTouristViewModel(int checkpointId, int startTourDateId, bool demo)
        {
            tourGuestService = new TourGuestService(Injector.CreateInstance<ITourGuestRepository>());
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(),
                new TourService(Injector.CreateInstance<ITourRepository>(), 
                new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))));

            tourGuests = new ObservableCollection<TourGuestDTO>();
            CheckTouristCommand = new RelayCommand(CheckTourist);
            this.checkpointId = checkpointId;
            this.startTourDateId = startTourDateId;
            CancelCommand = new RelayCommand(CloseWindow);
            OkCommand = new RelayCommand(Ok);
            StopDemoCommand = new RelayCommand(StopDemo);
            CheckBoxCheckedCommand = new RelayCommand(CheckBoxChecked);
            CheckBoxUncheckedCommand = new RelayCommand(CheckBoxUnchecked);
            UpdateTourists();
            ActivatedDemo = demo;
            if(demo)
                Demo();
        }

        public async void Demo()
        {
            await Task.Delay(1000);
            window = (CheckTouristForm)System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);
            int index = 0;
            if(tourGuests.Count == 0)
            {
                window.OkButton.Focus();
                await Task.Delay(1000);
                CloseWindow();
            }
            
            foreach(var guest in tourGuests)
            {
                if (stopDemo)
                    break;
                SelectItem(index);
                if (stopDemo)
                    break;
                index++;
                await Task.Delay(1000);
                if (stopDemo)
                    break;
                CheckBoxChecked();
                await Task.Delay(1000);
            }
            //await Task.Delay(1000);
            window.OkButton.Focus();
            await Task.Delay(1000);
            CloseWindow();
        }
        private void StopDemo()
        {
            stopDemo = true;
        }


        private void SelectItem(int index)
        {
            slectedTourGuest = tourGuests[index];
            window.TouristsDataGrid.Focus();
            window.TouristsDataGrid.UpdateLayout();
            window.TouristsDataGrid.ScrollIntoView(slectedTourGuest);
            var row = (DataGridRow)window.TouristsDataGrid.ItemContainerGenerator.ContainerFromItem(slectedTourGuest);
            if (row != null)
            {
                row.IsSelected = true;
            }
        }
        public void CloseWindow()
        {
            var data = "";
            if (stopDemo)
                data = "StopDemo";
            else
                data = "NotStopDemo";
            Messenger.Default.Send(new NotificationMessage<object>(data, "FollowKey"));
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
        public void UpdateTourists()
        {
            tourGuests.Clear();
            foreach (TourReservation tourReservation in tourReservationService.GetAll())
            {
                if (tourReservation.StartTourDateId == startTourDateId)
                {
                    LoadTodaysTourist(tourReservation);
                }
            }
        }

        private void LoadTodaysTourist(TourReservation tourReservation)
        {
            foreach (TourGuest tourGuest in tourGuestService.GetAll())
            {
                if (tourGuest.TourReservationId == tourReservation.Id && tourGuest.CheckPointId == -1)
                {
                    tourGuests.Add(new TourGuestDTO(tourGuest));
                }
            }
        }

        public void CheckBoxChecked()
        {
            slectedTourGuest.Checked = true;

        }

        public void CheckBoxUnchecked()
        {
            slectedTourGuest.Checked = false;
        }

        private void SaveTourGestCheckpoints()
        {
            foreach (TourGuestDTO tourGuest in tourGuests)
            {
                if (tourGuest.Checked == true)
                {
                    tourGuest.CheckPointId = checkpointId;
                    tourGuestService.Update(tourGuest.ToTourGuest());
                }
            }
        }
        public void Ok()
        {
            SaveTourGestCheckpoints();
            CloseWindow();
        }

        private void CheckTourist()
        {
            if(slectedTourGuest == null)
            {
                MessageBox.Show("You have to select a tourist to check him.");
                return;
            }
            slectedTourGuest.Checked = true;
        }
    }
}
