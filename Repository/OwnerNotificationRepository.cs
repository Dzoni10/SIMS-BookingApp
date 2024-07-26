using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class OwnerNotificationRepository : IOwnerNotificationRepository
    {
        Repository<OwnerNotification> ownerNotificationRepository;
        public Subject ownerNotificationSubject;

        public OwnerNotificationRepository()
        {
            ownerNotificationRepository = new Repository<OwnerNotification>();
            ownerNotificationSubject = new Subject();
        }

        public List<OwnerNotification> GetAll()
        {
            return ownerNotificationRepository.GetAll();
        }

        public void Delete(OwnerNotification ownerNotification)
        {
            ownerNotificationRepository.Delete(ownerNotification);
        }
        public OwnerNotification Save(OwnerNotification ownerNotification)
        {
            return ownerNotificationRepository.Save(ownerNotification);
        }
        public void SaveAll(List<OwnerNotification> ownerNotifications)
        {
            ownerNotificationRepository.SaveAll(ownerNotifications);
        }
        public void Update(OwnerNotification ownerNotification)
        {
            ownerNotificationRepository.Update(ownerNotification);
            ownerNotificationSubject.NotifyObservers();
        }
        public OwnerNotification GetById(int id)
        {
            return ownerNotificationRepository.FindId(id);
        }
        public int SavedOwnerNotificationId(OwnerNotification ownerNotification)
        {
            ownerNotificationRepository.Save(ownerNotification);
            return ownerNotification.Id;
        }
    }
}
