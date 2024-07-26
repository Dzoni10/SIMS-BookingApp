using BookingApp.Domain.Models;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Utilities;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;
using BookingApp.Application.UseCases;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
using System.Windows.Media;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModel
{
    public class AccommodationReservationsViewModel : INotifyPropertyChanged
    {
        public AccommodationReservations page;
        private readonly GuestWindow parentWindow;
        private readonly User user;
        private AccommodationReservationService accommodationReservationService { get; set; }
        private AccommodationsService accommodationsService { get; set; }
        private CanceledReservationService canceledReservationService { get; set; }
        private OwnerNotificationService ownerNotificationService { get; set; }
        private ImageService imageService { get; set; }
        private ReportGeneratorService reportGeneratorService { get; set; }
        public ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        public AccommodationReservationDTO SelectedReservation { get; set; }

        public RelayCommand CancelReservationCommand { get; }
        public RelayCommand EditReservationCommand { get; }
        public RelayCommand GenerateReportCommand { get; }


        private string reservationFeedbackMessage;
        public string ReservationFeedbackMessage
        {
            get { return reservationFeedbackMessage; }
            set
            {
                if (value != reservationFeedbackMessage)
                {
                    reservationFeedbackMessage = value;
                    OnPropertyChanged("ReservationFeedbackMessage");
                }
            }
        }

        public AccommodationReservationsViewModel(GuestWindow parentWindow, User user, AccommodationReservations page, string feedbackMessage)
        {
            this.page = page;
            this.parentWindow = parentWindow;
            this.user = user;

            accommodationReservationService = new AccommodationReservationService();
            accommodationsService = new AccommodationsService();
            canceledReservationService = new CanceledReservationService();
            ownerNotificationService = new OwnerNotificationService();
            imageService = new ImageService();
            reportGeneratorService = new ReportGeneratorService();

            Reservations = new ObservableCollection<AccommodationReservationDTO>();

            ReservationFeedbackMessage = feedbackMessage;

            CancelReservationCommand = new RelayCommand(CancelReservationExecute, CanExecute);
            EditReservationCommand = new RelayCommand(EditReservationExecute, CanExecute);
            GenerateReportCommand = new RelayCommand(GenerateReportExecute, CanExecute);

            LoadReservations();
        }
        public bool CanExecute()
        {
            return true;
        }
        public void CancelReservationExecute()
        {
            if (!accommodationReservationService.IsReservationCancelingAvailable(SelectedReservation.StartDate, SelectedReservation.AccommodationId))
            {
                ReservationFeedbackMessage = "You are too late to cancel the reservation";
                page.ReservationFeedbackMessage.Foreground = Brushes.Red;
            }
            else
            {
                ownerNotificationService.Save(new OwnerNotification(SelectedReservation.Id, NotificationStatus.Deleted));
                accommodationReservationService.Delete(SelectedReservation.ToAccommodationReservation());
                canceledReservationService.Save(new CanceledReservation(SelectedReservation.Id, SelectedReservation.AccommodationId, SelectedReservation.StartDate, SelectedReservation.EndDate));
                Reservations.Remove(SelectedReservation);

                ReservationFeedbackMessage = "Reservation successfuly canceled";
                page.ReservationFeedbackMessage.Foreground = Brushes.Green;

            }
        }
        public void EditReservationExecute()
        {
            parentWindow.MainFrame.NavigationService.Navigate(new ReservationAccommodationEditForm(parentWindow, user, SelectedReservation.ToAccommodationReservation()));
        }
        public void GenerateReportExecute()
        {
            //reportGeneratorService.GenerateReport(SelectedReservation.ToAccommodationReservation());
            GeneratePDF();
        }
        private void GeneratePDF()
        {
            // Kreiramo i konfiguriramo SaveFileDialog
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
                reportGeneratorService.GeneratePDF(filePath, SelectedReservation.Id);

                ReservationFeedbackMessage = "PDF saved: " + filePath;
                page.ReservationFeedbackMessage.Foreground = Brushes.Green;

                //MessageBox.Show("PDF saved: " + filePath);
                OpenPdf(filePath);
            }

        }

        private void OpenPdf(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                ReservationFeedbackMessage = "Unable to open PDF file: " + ex.Message;
                page.ReservationFeedbackMessage.Foreground = Brushes.Red;
            }
        }


        public void LoadReservations()
        {
            Reservations.Clear();
            foreach (AccommodationReservation reservation in accommodationReservationService.GetReservationsForUser(user))
            {
                AccommodationReservationDTO reservationDTO = new AccommodationReservationDTO(reservation);
                reservationDTO.AccommodationName = accommodationsService.GetById(reservation.AccommodationId).Name;
                reservationDTO.Image = imageService.GetImageForAccommodation(reservationDTO.AccommodationId);
                reservationDTO.PossibleCancellation = accommodationReservationService.IsReservationCancelingAvailable(reservation.StartDate, reservation.AccommodationId);
                Reservations.Add(reservationDTO);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
