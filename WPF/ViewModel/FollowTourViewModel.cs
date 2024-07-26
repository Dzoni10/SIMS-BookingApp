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

namespace BookingApp.WPF.ViewModel
{
    public class FollowTourViewModel : INotifyPropertyChanged
    {
        private FollowTourService followTourService;

        private TourService tourService;
        private CheckpointService checkpointService;
        private StartTourDateService startTourDateService;
        private UserService userService;
        private VoucherService voucherService;
        private NotificationService notificationService;
        public ObservableCollection<TourDTO> Tours { get; set; }
        public ObservableCollection<CheckpointDTO> Checkpoints { get; set; }
        public ObservableCollection<CheckpointDTO> SavedCheckpoints { get; set; }
        public CheckpointDTO SelectedCheckpoint { get; set; }
        public TourDTO SelectedTour { get; set; }
        public TourDTO SavedTour { get; set; }
        public string Date { get; set; }
       
        private int userId;
        
        public RelayCommand StartTourCommand { get;}
        public RelayCommand FinishTourCommand { get;}
        public RelayCommand CheckCommand { get;}
        public RelayCommand HelpCommand { get; }
        public RelayCommand CloseHelpCommand { get; }
        public RelayCommand CloseWindowCommand { get; }
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
        private FollowTourWindow window;
        public bool ActivatedDemo {  get; set; }
        private int currentCheckpoint;
        public FollowTourViewModel(int userId)
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())));
            followTourService = new FollowTourService(new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), tourService),
                new NotificationService(Injector.CreateInstance<INotificationRepository>()));
            startTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            checkpointService = new CheckpointService(Injector.CreateInstance<ICheckpointRepository>());
            userService = new UserService();
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>(), userService, startTourDateService,
                new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), tourService));
            notificationService = new NotificationService(Injector.CreateInstance<INotificationRepository>());

            StartTourCommand = new RelayCommand(StartTour, CanExecute);
            FinishTourCommand = new RelayCommand(FinishTour, CanExecute);
            CheckCommand = new RelayCommand(SetCheckpoint, CanExecute);
            HelpCommand = new RelayCommand(Help, CanExecute);
            CloseHelpCommand = new RelayCommand(CloseHelp, CanExecute);
            CloseWindowCommand = new RelayCommand(CloseWindow, CanExecute);
            DemoCommand = new RelayCommand(Demo, CanExecute);
            StopDemoCommand = new RelayCommand(StopDemo, CanExecute);

            Tours = new ObservableCollection<TourDTO>();
            Checkpoints = new ObservableCollection<CheckpointDTO>();
            SavedCheckpoints = new ObservableCollection<CheckpointDTO>();
            this.userId = userId;
            Date = DateTime.Now.Date.ToString("dd.MM.yyyy");
            UpdateTours();
            if(startTourDateService.GetActiveTour() != null)
            {
                SelectedTour = new TourDTO(tourService.GetById(startTourDateService.GetActiveTour().TourId));
                SelectedTour.StartTourDate = startTourDateService.GetActiveTour();
                LoadCheckpointsForSelectedTour();
                SetFlags();
            }
        }
        private bool CanExecute()
        {
            return true;
        }

        public void UpdateTours()
        {
            Tours.Clear();
            foreach (StartTourDate startDate in startTourDateService.GetFollowingTours())
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
                Tours.Add(tourDTO);
            }
        }

        public void StartTour()
        {
            if (!ActivatedDemo)
            {     
                    if (SelectedTour == null)
                    {
                        MessageBox.Show("You have not selected a tour!");
                        return;
                    }
                    if (startTourDateService.GetAll().Count(date => date.Status == TourStatus.Active) != 0)
                    {
                        MessageBox.Show("You cannot start a new tour while one is in progress.");
                        SelectedTour = SavedTour;
                        return;
                    }
            }
            SavedTour = SelectedTour;
            LoadCheckpointsForSelectedTour();
            SetCurrentCheckpoint(SelectedTour.StartTourDate.Id, Checkpoints[0].Id);
            SetFlags();
        }

        private void SetCurrentCheckpoint(int startTourDateId, int checkpointId)
        {
            SelectedTour.StartTourDate = followTourService.SetCurrentCheckpoint(startTourDateId, checkpointId);
        }
        private void LoadCheckpointsForSelectedTour()
        {
            Checkpoints.Clear();
            foreach (Checkpoint checkpoint in checkpointService.GetAllByTourId(SelectedTour.Id))
            {
                Checkpoints.Add(new CheckpointDTO(checkpoint));      
            }
        }

        private void SetFlags()
        {
            if (startTourDateService.GetById(SelectedTour.StartTourDate.Id).CurrentCheckpoint == 0) return;

            Checkpoints.Where(checkpoint => checkpoint.Id <= startTourDateService.GetById(SelectedTour.StartTourDate.Id).CurrentCheckpoint)
           .ToList()
           .ForEach(checkpoint => checkpoint.IsChecked = true);
        }

        private bool IsAllChecked()
        {
            return Checkpoints.Count > 0 && Checkpoints.All(checkpoint => checkpoint.IsChecked);
        }

        public void SetCheckpoint()
        {
            if (SelectedCheckpoint == null)
            {
                MessageBox.Show("You have not selected a checkpoint.");
                return;
            }
           
            Checking();
            SetFlags();
           
            if (IsAllChecked())
            {
                MessageBox.Show("Congratulations!!! You have successfully completed the tour.");
                Finish();
            }
        }

        private void Checking()
        {
            int i = Checkpoints.IndexOf(SelectedCheckpoint);
            if (i == 0 || (Checkpoints[i - 1].IsChecked && i > 0))
            {
                CheckTouristForm checkTouristForm = new CheckTouristForm(SelectedCheckpoint.Id, SelectedTour.StartTourDate.Id, ActivatedDemo);
                Messenger.Default.Register<NotificationMessage<object>>(this, OnDataReceived);
                checkTouristForm.ShowDialog();
                Checkpoints[i].IsChecked = true;
                SetCurrentCheckpoint(SelectedTour.StartTourDate.Id, SelectedCheckpoint.Id);
                LoadCheckpointsForSelectedTour();
                SetFlags();
            }
            else
            {
                MessageBox.Show("Go in order! Don't skip checkpoints");
            }
        }
        private void OnDataReceived(NotificationMessage<object> message)
        {
            var data = message.Content;
            string mess = Convert.ToString(data);
            if (mess == "StopDemo")
                ActivatedDemo = false;
        }

        public void FinishTour()
        {
            if (SelectedTour == null || SelectedTour.StartTourDate.Status != TourStatus.Active)
            {
                MessageBox.Show("To stop the tour, you need to start it.");
                SelectedTour = SavedTour;
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to end the tour?", "End tour", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)   Finish();
        }
        private void Finish()
        {
            Checkpoints.Clear();
            SetCurrentCheckpoint(SelectedTour.StartTourDate.Id, 0);
            followTourService.FinishTour(SelectedTour.StartTourDate.Id);
            followTourService.SendNotification(SelectedTour.StartTourDate.Id);
            AwardVoucherIfNecessary();
            Tours.Remove(SelectedTour);
            SelectedTour = null;
        }

        private void AwardVoucherIfNecessary()
        {
            userService.IncreaseReservationCount(4);
            List<int> notifyingUserIds = voucherService.PossiblyAwardVoucher(userId, SelectedTour.StartTourDate.Id);
            if(notifyingUserIds.Count > 0)
            {
                foreach(int id in notifyingUserIds)
                {
                    notificationService.Save(new Notification(id, "Congratulations, you just received a voucher for a free tour! Head to our menu to learn more.", NotificationType.Voucher));
                }
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

        private async void Demo()
        {
            if (Tours.Count == 0)
                return;
            SaveApplicationSatate();
            ActivatedDemo = true;
            await Task.Delay(1000);
            SetWindow();
            window.DemoLabel.Visibility = Visibility.Visible;
            window.DemoButton.Visibility = Visibility.Hidden;
            while (ActivatedDemo)
            {
                SelectTour();
                if (!ActivatedDemo)
                {
                    break;
                }
                await Task.Delay(1000);
                window.StartTour.Focus();
                await Task.Delay(1000);
                StartTour();
                if (!ActivatedDemo)
                {
                    break;
                }
                await Task.Delay(1000);
                int checks = Checkpoints.Count;
                if (!ActivatedDemo)
                {
                    break;
                }
                for (int i = 0; i < checks; i++)
                {
                    if (!ActivatedDemo)
                    {
                        break;
                    }
                    SelectCheckpoint(i);
                    await Task.Delay(1000);
                    window.CheckButton.Focus();
                    await Task.Delay(500);
                    if (!ActivatedDemo)
                    {
                        break;
                    }
                    Checking();
                    if (!ActivatedDemo)
                    {
                        break;
                    }
                    await Task.Delay(1000);                   
                }
                ResetDemo();
                if (!ActivatedDemo)
                {
                    break;
                }
            }
            window.DemoLabel.Visibility = Visibility.Hidden;
            window.DemoButton.Visibility = Visibility.Visible;
            ResetDemo();
        }

        public void SaveApplicationSatate()
        {
            SavedCheckpoints = Checkpoints;
            Checkpoints.Clear();
            SavedTour = SelectedTour;
            if(SelectedTour == null)
            {
                currentCheckpoint = 0;
            }
            else
            {
                currentCheckpoint = startTourDateService.GetById(SelectedTour.StartTourDate.Id).CurrentCheckpoint;
            }
        }
        private void ResetDemo()
        {
            if(SelectedTour == null)
            {
                return;
            }
            var date = startTourDateService.GetById(SelectedTour.StartTourDate.Id);
           
            date.CurrentCheckpoint = currentCheckpoint;
            if(currentCheckpoint == 0)
            {
                Checkpoints.Clear(); 
                date.Status = TourStatus.Reserved;
            }
            else
            {
                Checkpoints = SavedCheckpoints;
                date.Status = TourStatus.Active;              
                LoadCheckpointsForSelectedTour();
                SetFlags();
            }
                
            startTourDateService.Update(date);
            SelectedTour = SavedTour;     
        }

        private void SelectTour()
        {
            SelectedTour = Tours.First();
            window.TourDataGrid.Focus();
            window.TourDataGrid.UpdateLayout();
            window.TourDataGrid.ScrollIntoView(SelectedTour);
            var row = (DataGridRow)window.TourDataGrid.ItemContainerGenerator.ContainerFromItem(SelectedTour);
            if (row != null)
            {
                row.IsSelected = true;
            }
        }
        private void SelectCheckpoint(int i)
        {
            SelectedCheckpoint = Checkpoints[i];
            window.CheckpointDataGrid.Focus();
            window.CheckpointDataGrid.UpdateLayout();
            window.CheckpointDataGrid.ScrollIntoView(SelectedCheckpoint);
            var row = (DataGridRow)window.CheckpointDataGrid.ItemContainerGenerator.ContainerFromItem(SelectedCheckpoint);
            if (row != null)
            {
                row.IsSelected = true;
            }
        }
        private void StopDemo()
        {
            ActivatedDemo = false;
        }
        private void SetWindow()
        {
            window = (FollowTourWindow)System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);
        }
        private void CloseWindow()
        {
            Messenger.Default.Unregister<NotificationMessage<object>>(this, OnDataReceived);
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
