using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class ResignService
    {
        private StartTourDateService startTourDateService;
        private VoucherService voucherService;
        private TourService tourService;
        private TourReservationService tourReservationService;
        private NotificationService notificationService;
        private TourRequestService tourRequestService;
        public ResignService(StartTourDateService startTourDateService, VoucherService voucherService, TourService tourService, TourReservationService tourReservationService, NotificationService notificationService, TourRequestService tourRequestService) 
        {
            this.startTourDateService = startTourDateService;
            this.voucherService = voucherService;
            this.tourService = tourService;
            this.tourReservationService = tourReservationService;
            this.notificationService = notificationService;
            this.tourRequestService = tourRequestService;
        }

        public void Resign(int guideId)
        {
            foreach(var startTourDate in startTourDateService.GetForGuide(tourService.GetToursByGuideId(guideId)))
            {
                if(startTourDate.Status == TourStatus.Reserved)
                {
                    startTourDate.Status = TourStatus.Cancelled;
                    startTourDateService.Update(startTourDate);
                    AwardingVouchersForReservations(guideId, startTourDate.Id);
                }
                if(startTourDate.Status == TourStatus.Request)
                {
                    startTourDate.Status = TourStatus.Cancelled;
                    startTourDateService.Update(startTourDate);
                    AwardingVouchersForRequest(guideId, startTourDate.TourId);
                }
            }
            UpdateVouchers(guideId);
        }

        public void AwardingVouchersForRequest(int guideId, int requestId)
        {
            TourRequest tourRequest = tourRequestService.GetById(requestId);
            voucherService.Save(new Voucher(guideId, tourRequest.RequestingUser.Id, DateTime.Now.AddYears(2)));
            notificationService.Save(new Notification(tourRequest.RequestingUser.Id, "Congratulations, you just received a voucher for a free tour! Head to our menu to learn more.", NotificationType.Voucher));           
        }
        public void AwardingVouchersForReservations(int guideId, int startTourDateId)
        {
            List<TourReservation> reservations = tourReservationService.GetByStartTourDateId(startTourDateId);
            foreach (TourReservation reservation in reservations)
            {              
                voucherService.Save(new Voucher(guideId, reservation.User.Id, DateTime.Now.AddYears(2)));
                notificationService.Save(new Notification(reservation.User.Id, "Congratulations, you just received a voucher for a free tour! Head to our menu to learn more.", NotificationType.Voucher));
            }
        }

        public void UpdateVouchers(int guideId)
        {
            foreach(var voucher in voucherService.GetAllForGuide(guideId))
            {
                voucher.GuideId = 0;
                voucherService.Update(voucher);
            }
        }
    }
}
