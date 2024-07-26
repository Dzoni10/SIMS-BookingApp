using BookingApp.Domain.Models;
using BookingApp.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel
{
    public class VoucherItemViewModel : UserControl
    {

        public VoucherItem parentWindow { get; set; }

        public static readonly DependencyProperty VoucherNameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(RateTourControl), new PropertyMetadata(null));

        public string Name
        {
            get { return (string)GetValue(VoucherNameProperty); }
            set { SetValue(VoucherNameProperty, value); }
        }

        public static readonly DependencyProperty ExpirationDateProperty =
            DependencyProperty.Register("ExpirationDate", typeof(DateTime), typeof(RateTourControl), new PropertyMetadata(null));

        public User LoggedInUser { get; set; }

        public DateTime ExpirationDate
        {
            get { return (DateTime)GetValue(ExpirationDateProperty); }
            set { SetValue(ExpirationDateProperty, value); }
        }

        public VoucherItemViewModel(VoucherItem voucherItem, User user)
        {
            parentWindow = voucherItem;
            LoggedInUser = user;
        }
    }
}
