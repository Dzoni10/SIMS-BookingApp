using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class CancelTourService
    {
        private StartTourDateService startTourDateService;
        private TourReservationService tourReservationService;
        private VoucherService voucherService;
        private NotificationService notificationService;
        public CancelTourService(StartTourDateService startTourDateService, TourReservationService tourReservationService, VoucherService voucherService, NotificationService notificationService)
        {
            this.startTourDateService = startTourDateService;
            this.tourReservationService = tourReservationService;
            this.voucherService = voucherService;
            this.notificationService = notificationService;
        }

        public void RecordCancelled(int startTourDateId)
        {
            StartTourDate date = startTourDateService.GetById(startTourDateId);
            date.Status = TourStatus.Cancelled;
            startTourDateService.Update(date);
        }
        public void AwardingVouchers(int userId, int startTourDateId)
        {
            List<TourReservation> reservations = tourReservationService.GetByStartTourDateId(startTourDateId);
            foreach (TourReservation reservation in reservations)
            {
                voucherService.Save(new Voucher(userId, reservation.User.Id, DateTime.Now.AddYears(1)));
                notificationService.Save(new Notification(reservation.User.Id, "Congratulations, you just received a voucher for a free tour! Head to our menu to learn more.", NotificationType.Voucher));
            }
        }

        public void Canacel(int userId, int startTourDateId)
        {
            AwardingVouchers(userId, startTourDateId);
            RecordCancelled(startTourDateId);
        }
    }
}
