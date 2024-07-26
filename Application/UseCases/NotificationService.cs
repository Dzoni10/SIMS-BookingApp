using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class NotificationService
    {

        private INotificationRepository notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public Notification Save(Notification notification)
        {
            return notificationRepository.Save(notification);
        }
        public void SaveAll(List<Notification> notifications)
        {
            notificationRepository.SaveAll(notifications);
        }
        public void Update(Notification notifications)
        {
            notificationRepository.Update(notifications);
        }
        public Notification GetById(int id)
        {
            return notificationRepository.GetById(id);
        }
        public List<Notification> GetAll()
        {
            return notificationRepository.GetAll();
        }
    }
}
