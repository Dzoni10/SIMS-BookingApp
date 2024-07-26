using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;

namespace BookingApp.Repository
{
    public class OwnerGuestNotificationRepository : IOwnerGuestNotificationRepository
    {
        Repository<OwnerGuestNotification> ownerGuestNotificationRepository;
        public Subject ownerGuestNotificationSubject;

        public OwnerGuestNotificationRepository()
        {
            ownerGuestNotificationRepository = new Repository<OwnerGuestNotification>();
            ownerGuestNotificationSubject = new Subject();
        }

        public void Save(OwnerGuestNotification ownerGuestNotification)
        {
            ownerGuestNotificationRepository.Save(ownerGuestNotification);
            ownerGuestNotificationSubject.NotifyObservers();
        }

        public void SaveAll(List<OwnerGuestNotification> ownerGuestNotifications)
        {
            ownerGuestNotificationRepository.SaveAll(ownerGuestNotifications);
            ownerGuestNotificationSubject.NotifyObservers();
        }
        public void Update(OwnerGuestNotification ownerGuestNotification)
        {
            ownerGuestNotificationRepository.Update(ownerGuestNotification);
            ownerGuestNotificationSubject.NotifyObservers();
        }

        public void Delete(OwnerGuestNotification ownerGuestNotification)
        {
            ownerGuestNotificationRepository.Delete(ownerGuestNotification);
            ownerGuestNotificationSubject.NotifyObservers();
        }

        public OwnerGuestNotification GetById(int id)
        {
            return ownerGuestNotificationRepository.FindId(id);
        }

        public List<OwnerGuestNotification> GetAll()
        {
             return ownerGuestNotificationRepository.GetAll(); 

        }
    }

}
