using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
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
    public class ShowVouchersWindowViewModel
    {
        public ShowVouchersWindow parentWindow { get; set; }

        public ObservableCollection<VoucherItem> Vouchers { get; set; }
        private VoucherService VoucherService;
        public User LoggedInUser { get; set; }

        public ShowVouchersWindowViewModel(ShowVouchersWindow showVouchersWindow, User user)
        {
            parentWindow = showVouchersWindow;
            VoucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>(), 
                             new UserService(), 
                             new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()),
                             new TourReservationService(Injector.CreateInstance<ITourReservationRepository>(), 
                             new TourService(Injector.CreateInstance<ITourRepository>(), 
                             new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>()), 
                             new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), 
                             new TourRequestGuestService(Injector.CreateInstance<ITourRequestGuestRepository>())))));
            Vouchers = new ObservableCollection<VoucherItem>();
            LoggedInUser = user;
            Update();
        }

        private void Update()
        {
            foreach (Voucher voucher in VoucherService.GetAll())
            {
                if (voucher.TouristId == LoggedInUser.Id && voucher.Status == VoucherStatus.Valid)
                {
                    var voucherItem = new VoucherItem(LoggedInUser);
                    voucherItem.VoucherName.Content = $"Voucher #{voucher.Id}";
                    voucherItem.ExpirationDateLabel.Content = $"Expiration date: {voucher.ExpirationDate.ToString("dd.MM.yyyy HH:mm")}";
                    Vouchers.Add(voucherItem);
                }
            }
        }

    }
}
