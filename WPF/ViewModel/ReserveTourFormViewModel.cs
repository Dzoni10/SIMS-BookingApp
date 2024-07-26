using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using BookingApp.Utilities;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Diagnostics;
using Notifications.Wpf.Core;

namespace BookingApp.WPF.ViewModel
{
    public class ReserveTourFormViewModel
    {

        public ReserveTourForm parentWindow { get; set; }
        public ShowToursViewModel otherViewModel { get; set; }
        public ReserveTourViewModel reserveTourViewModel { get; set; }

        public event EventHandler Cancelled;

        public int numberOfReservations;
        public int numberOfReservationsCounter;
        public User LoggedInUser { get; set; }
        public StartTourDateDTO SelectedStartTourDate { get; set; }
        public VoucherDTO SelectedVoucher { get; set; }
        public ObservableCollection<TourGuestDTO> SignedUpTourists { get; set; }
        public ObservableCollection<VoucherDTO> Vouchers { get; set; }
        private VoucherService voucherService;
        private TourGuestService tourGuestService;
        private TourReservationService tourReservationService;
        private StartTourDateService startTourDateService;
        public TourGuestDTO tourGuest { get; set; }

        public RelayCommand EnterCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand PDFCommand { get; set; }

        public ReserveTourFormViewModel(ReserveTourForm reserveTourForm, User user, StartTourDateDTO selectedStartTourDate, int numOfReservation, int numOfReservationsCounter, ObservableCollection<TourGuestDTO> tourists, ShowToursViewModel viewModel, ReserveTourViewModel reserveViewModel)
        {
            parentWindow = reserveTourForm;
            otherViewModel = viewModel;
            reserveTourViewModel = reserveViewModel;
            LoggedInUser = user;
            tourGuestService = new TourGuestService(Injector.CreateInstance<ITourGuestRepository>());
            tourGuest = new TourGuestDTO();
            tourGuest.Age = 12;
            startTourDateService = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
            new VoucherService(Injector.CreateInstance<IVoucherRepository>(), new UserService(), startTourDateService,
                new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), new TourService(Injector.CreateInstance<ITourRepository>(), startTourDateService, new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())))));
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(),
                new TourService(Injector.CreateInstance<ITourRepository>(), startTourDateService, new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),
                new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>()))));
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>(),
                new UserService(Injector.CreateInstance<IUserRepository>()), startTourDateService, tourReservationService);
            numberOfReservations = numOfReservation;
            SignedUpTourists = tourists;
            SelectedStartTourDate = selectedStartTourDate;
            numberOfReservationsCounter = numOfReservationsCounter;
            Vouchers = new ObservableCollection<VoucherDTO>();
            UpdateVouchers();
            if (numberOfReservationsCounter > numberOfReservations)
            {
                ShowInSpecialCase();
            }
            parentWindow.Header.Text = $"{numberOfReservationsCounter}. tourist";

            EnterCommand = new RelayCommand(EnterClick);
            ReserveCommand = new RelayCommand(ReserveClick);
            CancelCommand = new RelayCommand(CancelClick);
            PDFCommand = new RelayCommand(PDFClick);
        }


        private void UpdateVouchers()
        {
            foreach (Voucher voucher in voucherService.GetAll())
            {
                if (voucher.TouristId == LoggedInUser.Id && voucher.Status == VoucherStatus.Valid)
                    Vouchers.Add(new VoucherDTO(voucher));
            }
        }

        private void CancelClick()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(parentWindow);
            navigationService.Navigate(new ReserveTour(SelectedStartTourDate, LoggedInUser, otherViewModel));
        }

        private void ShowSuccessfullMessage()
        {
            parentWindow.ReserveButton.Visibility = Visibility.Collapsed;
            parentWindow.TouristsDataGrid.Visibility = Visibility.Collapsed;
            parentWindow.AreYouSureMessage.Visibility = Visibility.Collapsed;
            parentWindow.AreYouSureMessage.Visibility = Visibility.Collapsed;
            parentWindow.TouristsVouchers.Visibility = Visibility.Collapsed;
            parentWindow.CancelButton.Visibility = Visibility.Collapsed;
            parentWindow.SuccessfullMessage.Visibility = Visibility.Collapsed;
            parentWindow.LikeImage.Visibility = Visibility.Visible;
            parentWindow.SuccessfullMessage.Visibility = Visibility.Visible;
            parentWindow.PdfButton.Visibility = Visibility.Visible;
        }

        private void ReserveClick()
        {
            int tourReservationId = WriteReservation();
            WriteTourGuests(tourReservationId);
            ChangeCapacityOfTour();
            ShowSuccessfullMessage();
            UseVoucher();
        }

        private void UseVoucher()
        {
            if (SelectedVoucher != null)
            {
                SelectedVoucher.Status = VoucherStatus.Used;
                voucherService.Update(SelectedVoucher.ToVoucher());
            }
        }

        private int WriteReservation()
        {
            TourReservation tourReservation = new TourReservation(LoggedInUser, SelectedStartTourDate.Id);
            tourReservation = tourReservationService.Save(tourReservation);
            return tourReservation.Id;
        }

        private void WriteTourGuests(int tourReservationId)
        {
            foreach (TourGuestDTO tourGuestDTO in SignedUpTourists)
            {
                tourGuestDTO.TourReservationId = tourReservationId;
                tourGuestDTO.CheckPointId = -1;
                tourGuestService.Save(tourGuestDTO.ToTourGuest());
            }
        }

        private void ChangeCapacityOfTour()
        {
            SelectedStartTourDate.Capacity -= numberOfReservations;
            SelectedStartTourDate.Status = TourStatus.Reserved;
            startTourDateService.Update(SelectedStartTourDate.ToStartTourDate());
        }

        private void EnterClick()
        {
            if (tourGuest.FullName != null)
            {
                tourGuest.TourReservationId = SelectedStartTourDate.Id;
                SignedUpTourists.Add(tourGuest);
                numberOfReservationsCounter++;
                CollapseEverythingExceptFrame();
                ReserveTourForm newForm = new ReserveTourForm(SelectedStartTourDate, LoggedInUser, numberOfReservations, numberOfReservationsCounter, SignedUpTourists, otherViewModel, reserveTourViewModel);
                parentWindow.ReserveTourFrame.Content = newForm;
            }
            else
                reserveTourViewModel.SendValidationNotificationForReserveControl(MakeNotification());
        }

        private NotificationContent MakeNotification()
        {
            var content = new NotificationContent
            {
                Title = "Oh no!",
                Message = "Enter full name for your tourist!"
            };

            return content;
        }

        private void ShowInSpecialCase()
        {
            parentWindow.Header.Visibility = Visibility.Collapsed;
            parentWindow.AgeLabel.Visibility = Visibility.Collapsed;
            parentWindow.NumberOfGuestsTextBox.Visibility = Visibility.Collapsed;
            parentWindow.EnterButton.Visibility = Visibility.Collapsed;
            parentWindow.FullNameLabel.Visibility = Visibility.Collapsed;
            parentWindow.FullNameTextBox.Visibility = Visibility.Collapsed;
            parentWindow.TouristsVouchers.Visibility = Visibility.Collapsed;
            parentWindow.ReserveButton.Visibility = Visibility.Visible;
            parentWindow.AreYouSureMessage.Visibility = Visibility.Visible;
            parentWindow.TouristsVouchers.Visibility = Visibility.Visible;
            parentWindow.CancelButton.Visibility = Visibility.Visible;
            PositionDataGrid();
        }

        private void PositionDataGrid()
        {
            parentWindow.TouristsDataGrid.HorizontalAlignment = HorizontalAlignment.Center;
            parentWindow.TouristsDataGrid.VerticalAlignment = VerticalAlignment.Center;
        }

        private void CollapseEverythingExceptFrame()
        {
            parentWindow.Header.Visibility = Visibility.Collapsed;
            parentWindow.FullNameLabel.Visibility = Visibility.Collapsed;
            parentWindow.FullNameTextBox.Visibility = Visibility.Collapsed;
            parentWindow.AgeLabel.Visibility = Visibility.Collapsed;
            parentWindow.NumberOfGuestsTextBox.Visibility = Visibility.Collapsed;
            parentWindow.EnterButton.Visibility = Visibility.Collapsed;
            parentWindow.TouristsDataGrid.Visibility = Visibility.Collapsed;
        }

        private void PDFClick()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                Title = "Save PDF"
            };

            // Prikazujemo SaveFileDialog korisniku
            bool? result = saveFileDialog.ShowDialog();

            // Ako je korisnik pritisnuo "Save" dugme
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                // Generišemo PDF i čuvamo ga na odabranoj putanji
                StartTourDate startTourDate = SelectedStartTourDate.ToStartTourDate();
                List<TourGuest> guests = CreateList();
                Voucher voucher = GetVoucher();
                tourReservationService.GeneratePDF(filePath, startTourDate, guests, voucher);
                OpenPdf(filePath);
            }
        }

        private Voucher GetVoucher()
        {
            Voucher newVoucher = new Voucher();
            if (SelectedVoucher == null)
                return newVoucher;
            return SelectedVoucher.ToVoucher();
        }

        private List<TourGuest> CreateList()
        {
            List<TourGuest> list = new List<TourGuest>();
            foreach(TourGuestDTO guestDTO in SignedUpTourists)
            {
                TourGuest guest = guestDTO.ToTourGuest();
                list.Add(guest);
            }
            return list;
        }

        private void OpenPdf(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće otvoriti PDF fajl: " + ex.Message);
            }
        }
    }
}
