using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class FollowTourService
    {
        private StartTourDateService startTourDateService;
        private TourReservationService tourReservationService;
        private NotificationService notificationService;
        public FollowTourService(StartTourDateService startTourDateService, TourReservationService tourReservationService, NotificationService notificationService) 
        {
            this.startTourDateService = startTourDateService;
            this.tourReservationService = tourReservationService;
            this.notificationService = notificationService;
        }

        public StartTourDate SetCurrentCheckpoint(int startTourDateId, int checkpointId)
        {
            StartTourDate startTourDate = startTourDateService.GetById(startTourDateId);
            startTourDate.Status = TourStatus.Active;
            startTourDate.CurrentCheckpoint = checkpointId;
            startTourDateService.Update(startTourDate);
            return startTourDate;
        }

        public StartTourDate FinishTour(int startTourDateId)
        {
            StartTourDate startTourDate = startTourDateService.GetById(startTourDateId);
            startTourDate.Status = TourStatus.Finished;
            startTourDateService.Update(startTourDate);
            return startTourDate;
        }

        public void SendNotification(int startTourId)
        {
            foreach (TourReservation reservation in tourReservationService.GetByStartTourDateId(startTourId))
            {
                notificationService.Save(new Notification(reservation.User.Id, startTourId.ToString(), NotificationType.FinishedTour));
            }
        }

        /*public void SetFlags(int startTourId)
        {
            if (startTourDateService.GetById(SelectedTour.StartTourDate.Id).CurrentCheckpoint == 0) return;

            Checkpoints.Where(checkpoint => checkpoint.Id <= startTourDateService.GetById(SelectedTour.StartTourDate.Id).CurrentCheckpoint)
           .ToList()
           .ForEach(checkpoint => checkpoint.IsChecked = true);
        }*/


    }
}
