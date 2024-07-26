using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class OwnerGuestNotificationService
    {
        private IOwnerGuestNotificationRepository ownerGuestNotificationRepository;
        public OwnerGuestNotificationService() 
        {
            ownerGuestNotificationRepository = Injector.Injector.CreateInstance<IOwnerGuestNotificationRepository>();
        }

        public void SaveNotification(OwnerGuestNotification notification)
        {
            ownerGuestNotificationRepository.Save(notification);
        }
    }
}
