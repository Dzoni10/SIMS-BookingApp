using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class EditedReservationsService
    {
        public IEditedReservationRepository editedReservationRepository;
        private UserService userService { get; set; }
        private OwnerGuestNotificationRepository ownerGuestNotificationRepository { get; set; }
        public EditedReservationsService()
        {
            editedReservationRepository = Injector.Injector.CreateInstance<IEditedReservationRepository>();
            userService = new UserService();
            ownerGuestNotificationRepository = new OwnerGuestNotificationRepository();
        }
        public void Save(EditedReservation editedReservation)
        {
            editedReservationRepository.Save(editedReservation);
        }
        public void SaveAll(List<EditedReservation> editedReservations)
        {
            editedReservationRepository.SaveAll(editedReservations);
        }
        public void Update(EditedReservation editedReservation)
        {
            editedReservationRepository.Update(editedReservation);
        }
        public EditedReservation GetById(int id)
        {
            return editedReservationRepository.GetById(id);
        }
        public List<EditedReservation> GetAll()
        {
            return editedReservationRepository.GetAll();
        }
        public void Delete(EditedReservation editedReservations)
        {
            editedReservationRepository.Delete(editedReservations);
        }
        public List<EditedReservation> GetEditedReservationsForUser(User user)
        {
            int guestId = userService.GetGuestIdFromUser(user);
            List<EditedReservation> reservations = new List<EditedReservation>();

            foreach (EditedReservation reservation in editedReservationRepository.GetAll())
            {
                reservations.Add(reservation);
            }

            return reservations;
        }
        public void DeleteNotifications()
        {
            foreach (OwnerGuestNotification notification in ownerGuestNotificationRepository.GetAll())
            {
                ownerGuestNotificationRepository.Delete(notification);
            }
        }

    }
}
