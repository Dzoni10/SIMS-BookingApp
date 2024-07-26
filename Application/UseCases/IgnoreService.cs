using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models; 


namespace BookingApp.Application.UseCases
{
    public class IgnoreService
    {
        private IIgnoreRepository ignoreRepository;
        public OwnerGuestNotificationService ownerGuestNotificationService;

        public IgnoreService()
        {
            ignoreRepository = Injector.Injector.CreateInstance<IIgnoreRepository>();
            ownerGuestNotificationService = new OwnerGuestNotificationService();
        }

        public void Save(Ignore ignore)
        {
            ignoreRepository.Save(ignore);
        }
        public List<Ignore> GetAll()
        {
            return ignoreRepository.GetAll();
        }
        public void SaveNotification(OwnerGuestNotification notification)
        {
            ownerGuestNotificationService.SaveNotification(notification);
        }
    }
}
