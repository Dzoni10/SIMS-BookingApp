using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        Repository<Notification> notificationRepository;

        public NotificationRepository()
        {
            notificationRepository = new Repository<Notification>();
        }

        public Notification Save(Notification notification)
        {
            return notificationRepository.Save(notification);
        }

        public void SaveAll(List<Notification> notifications)
        {
            notificationRepository.SaveAll(notifications);
        }

        public void Update(Notification notification)
        {
            notificationRepository.Update(notification);
        }

        public Notification GetById(int id)
        {
            return notificationRepository.FindId(id);
        }

        public List<Notification> GetAll()
        {
            return notificationRepository.GetAll();
        }
    }
}
