using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;
using BookingApp.WPF.View;
using BookingApp.Utilities;
using System.Windows.Controls;
using System.Globalization;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using System.Data;

namespace BookingApp.WPF.ViewModel
{
    public class SelectDateTimeViewModel: ViewModelBase
    {
        SelectDateTimeForm window;
        public RelayCommand StopDemoCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand OkCommand { get; }
        public DateTime dateTime { get; set; }
        private DateTime demoDate;
        public bool ActivatedDemo { get; set; }
        public SelectDateTimeViewModel(SelectDateTimeForm window, DateTime dateFrom, DateTime dateTo, bool demo)
        {
            this.window = window;
            /* Onemogucen izbor proslih datuma */
            window.StartDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, dateFrom));
            window.StartDate.BlackoutDates.Add(new CalendarDateRange(dateTo, DateTime.Today.AddYears(100)));
            window.OkButton.IsEnabled = false;
            StopDemoCommand = new RelayCommand(StopDemo);
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
            demoDate = dateFrom.AddDays(1);
            ActivatedDemo = demo;
            if (demo)
                Demo();
        }

        public void Cancel()
        {
            window.Close();
        }

        private async void Demo()
        {
            await Task.Delay(1000);
            //window = (SelectDateTimeForm)System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);
            window.StartDate.IsDropDownOpen = true;
            window.StartDate.SelectedDate = demoDate;
            await Task.Delay(1000);
            window.StartDate.IsDropDownOpen = false;
            await Task.Delay(1000);
            window.StartTime.Focus();
            await Task.Delay(1000);
            window.StartTime.Value = DateTime.Now;
            await Task.Delay(1000);
            window.OkButton.Focus();
            await Task.Delay(1000);
            Ok();
        }
        private void StopDemo()
        { 
            var data = DateTime.MinValue;
            Messenger.Default.Send(new NotificationMessage<object>(data, "YourMessageKey"));
            window.Close();
        }
        private DateTime ParseTime()
        {
            string time = window.StartTime.Text.Trim();
            if (DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return dateTime;
        }

        public void Ok()
        {
            if (window.StartTime?.Text != null && window.StartDate?.Text != null)
            {
                dateTime = window.StartDate.SelectedDate.Value.Date + ParseTime().TimeOfDay;
                var data = dateTime;
                Messenger.Default.Send(new NotificationMessage<object>(data, "YourMessageKey"));
                window.Close();
            }
        }

        public void StartDateSelectedDateChanged()
        {
            if (window.StartDate.ToString() == "")
            {
                window.DateErrorLabel.Visibility = Visibility.Visible;
                window.OkButton.IsEnabled = false;
            }
            else
            {
                window.DateErrorLabel.Visibility = Visibility.Hidden;
                if (window.TimeErrorLabel.Visibility == Visibility.Hidden)
                {
                    window.OkButton.IsEnabled = true;
                }
            }
        }

        public void StartTimeValueChanged()
        {
            if (window.StartTime.ToString() == "" || window.StartTime == null)
            {
                window.TimeErrorLabel.Visibility = Visibility.Visible;
                window.OkButton.IsEnabled = false;
            }
            else
            {
                window.TimeErrorLabel.Visibility = Visibility.Hidden;
                if (window.DateErrorLabel.Visibility == Visibility.Hidden)
                {
                    window.OkButton.IsEnabled = true;
                }
            }
        }
    }
}
