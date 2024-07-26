using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel
{
    public class TouristNotificationWindowViewModel
    {

        public TouristNotificationWindow parentWindow { get; set; }
        public ObservableCollection<NotificationDTO> Notifications { get; set; }
        private NotificationService NotificationService { get; set; }
        public User LoggedInUser { get; set; }

        public TouristNotificationWindowViewModel(TouristNotificationWindow touristNotificationWindow, User user)
        {
            parentWindow = touristNotificationWindow;
            LoggedInUser = user;
            Notifications = new ObservableCollection<NotificationDTO>();
            NotificationService = new NotificationService(Injector.CreateInstance<INotificationRepository>());
            Update();
            SetNotificationControl();
        }

        private void Update()
        {
            foreach (Notification notification in NotificationService.GetAll())
            {
                if (notification.UserId == LoggedInUser.Id && notification.Status != NotificationState.Read)
                {
                    Notifications.Add(new NotificationDTO(notification));
                }
            }
        }

        public void DeleteAcceptedNotification(AcceptedTourRequestNotification card)
        {
            parentWindow.NotificationListView.Items.Remove(card);
            Notification not = NotificationService.GetById(card.NotificationId);
            not.Status = NotificationState.Read;
            NotificationService.Update(not);
        }

        public void DeleteVoucherNotification(VoucherNotification card)
        {
            parentWindow.NotificationListView.Items.Remove(card);
            Notification not = NotificationService.GetById(card.NotificationId);
            not.Status = NotificationState.Read;
            NotificationService.Update(not);
        }

        public void DeleteTouristAttendingNotification(TouristAttendingNotification card)
        {
            parentWindow.NotificationListView.Items.Remove(card);
            Notification not = NotificationService.GetById(card.NotificationId);
            not.Status = NotificationState.Read;
            NotificationService.Update(not);
        }

        public void DeleteNewTourNotification(NewTourNotification card)
        {
            parentWindow.NotificationListView.Items.Remove(card);
            Notification not = NotificationService.GetById(card.NotificationId);
            not.Status = NotificationState.Read;
            NotificationService.Update(not);
        }

        private void SetNotificationControl()
        {
            foreach(NotificationDTO notification in Notifications)
            {
                if(notification.Type == NotificationType.Voucher)
                {
                    var voucherNotification = new VoucherNotification(notification.Id, LoggedInUser, this);
                    voucherNotification.NotificationLabel.Text = notification.Text;
                    parentWindow.NotificationListView.Items.Add(voucherNotification);
                }
                else if(notification.Type == NotificationType.AcceptedTourRequest)
                {
                    var acceptedTourRequestnotification = new AcceptedTourRequestNotification(notification.Id, LoggedInUser, int.Parse(notification.Text), this);
                    parentWindow.NotificationListView.Items.Add(acceptedTourRequestnotification);
                }
                else if(notification.Type == NotificationType.FinishedTour)
                {
                    var touristAttendingNotification = new TouristAttendingNotification(notification.Id, LoggedInUser, int.Parse(notification.Text), this);
                    parentWindow.NotificationListView.Items.Add(touristAttendingNotification);
                }
                else if(notification.type == NotificationType.NewTour)
                {
                    var newTourNotification = new NewTourNotification(notification.Id, LoggedInUser, int.Parse(notification.Text), this);
                    parentWindow.NotificationListView.Items.Add(newTourNotification);
                }
            }
        }
       
    }
}
