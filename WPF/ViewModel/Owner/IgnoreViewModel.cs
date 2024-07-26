using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;
using BookingApp.DTO;
using BookingApp.Utilities;
using System.Windows.Navigation;
using BookingApp.WPF.View.Owner;
using BookingApp.Domain.Models;
using BookingApp.Application.UseCases;
using System.Collections.ObjectModel;
using BookingApp.Application.Injector;
using BookingApp.Domain.RepositoryInterfaces;
namespace BookingApp.WPF.ViewModel.Owner
{
    public class IgnoreViewModel
    {
        public IgnorePage page;
        public EditedReservationDTO editedReservation;
        public IgnoreService ignoreService;
        public RequestService requestService;
        public ObservableCollection<EditedReservationDTO> EditedReservations { get; set; }
        public RelayCommand BackCommand { get; }
        public RelayCommand SendCommand { get; }
        public IgnoreViewModel(IgnorePage page, EditedReservationDTO SelectedEditedReservation, ObservableCollection<EditedReservationDTO> EditedReservations)
        {
            this.page = page;
            editedReservation = SelectedEditedReservation;
            this.EditedReservations = EditedReservations;
            ignoreService = new IgnoreService();
            requestService = new RequestService();
            BackCommand = new RelayCommand(BackExecute);
            SendCommand = new RelayCommand(SendExecute);
        }
        public void BackExecute()
        {
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.GoBack();
        }
        private void SendExecute()
        {
            Ignore ignore = new Ignore();
            OwnerGuestNotification ownerGuestNotification;
            EditedReservation reservation;
            reservation = requestService.GetEditedReservationById(editedReservation.Id);
            reservation.Status = ReservationStatus.Rejected;
            requestService.UpdateEditedReservation(reservation);
            ignore.AccommodationId = editedReservation.AccommodationId;
            ignore.GuestId = editedReservation.GuestId;
            ignore.EditedReservationId = editedReservation.Id;
            ignore.Explanation = page.ExplanationTextBox.Text;
            ignoreService.Save(ignore);
            ownerGuestNotification = new OwnerGuestNotification(editedReservation.Id, ReservationStatus.Rejected);
            ignoreService.SaveNotification(ownerGuestNotification);
            page.ExplanationTextBox.Text = "";
            EditedReservations.Remove(editedReservation);
            NavigationService navigationService = NavigationService.GetNavigationService(page);
            navigationService.GoBack();
        }

    }
}
