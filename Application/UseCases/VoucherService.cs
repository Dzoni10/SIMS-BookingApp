using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.UseCases
{
    public class VoucherService
    {
        private IVoucherRepository voucherRepository;
        private UserService userService;
        private StartTourDateService startTourDateService;
        private TourReservationService tourReservationService;
        public VoucherService(IVoucherRepository voucherRepository, UserService userService, StartTourDateService startTourDateService, TourReservationService tourReservationService) 
        {
            this.voucherRepository = voucherRepository;
            this.userService = userService;
            this.startTourDateService = startTourDateService;
            this.tourReservationService = tourReservationService;
        }
        public void Save(Voucher voucher)
        {
            voucherRepository.Save(voucher);
        }

        public List<Voucher> GetAll()
        {
            return voucherRepository.GetAll();
        }

        public void Update(Voucher voucher)
        {
            voucherRepository.Update(voucher);
        }

        public List<int> PossiblyAwardVoucher(int userId, int startTourId)
        {
            List<TourReservation> reservations = tourReservationService.GetByStartTourDateId(startTourId);
            List<User> users = new List<User>();
            List<int> notifyingUsersIds = new List<int>();
            foreach(TourReservation tr in reservations)
            {
                if (users.Any(u => u.Id == tr.Id))
                    continue;
                User requestingUser = userService.GetById(tr.User.Id);
                users.Add(requestingUser);
            }
            foreach (User user in users)
            {
                if (user.ReservationCount == 5 && user.FirstReservationDate.AddYears(1) > DateTime.Now)
                {
                    Voucher voucher = new Voucher(userId, user.Id, DateTime.Now.AddMonths(6));
                    Save(voucher);
                    userService.RestoreOriginalFields(user);
                    notifyingUsersIds.Add(user.Id);
                }
            }
            return notifyingUsersIds;
        }

        public List<Voucher> GetAllForGuide(int guideId)
        {
            List<Voucher> vouchers = new List<Voucher>();
            foreach(var voucher in voucherRepository.GetAll())
            {
                if(voucher.GuideId == guideId) 
                {
                    vouchers.Add(voucher);
                }
            }
            return vouchers;
        }
    }
    
}
