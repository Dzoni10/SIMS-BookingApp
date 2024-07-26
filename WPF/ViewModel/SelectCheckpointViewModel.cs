using BookingApp.Utilities;
using BookingApp.WPF.View;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel
{

    public class SelectCheckpointViewModel
    {
        public string ChackpointName { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "ChackpointName")
                {
                    if (string.IsNullOrEmpty(ChackpointName))
                        return "Please enter the value.";
                }
                return null;
            }
        }
        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }
        private SelectCheckpointForm window;
        public SelectCheckpointViewModel()
        {
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
            
        }

        private void Ok()
        {
            if (ChackpointName.Length != 0)
            {
                var data = ChackpointName;
                Messenger.Default.Send(new NotificationMessage<object>(data, "SecondMessageKey"));
                //window = (BookingApp.WPF.View.SelectCheckpointForm)System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);
                System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
            }
        }
        private void Cancel()
        {
            System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive).Close();
        }
    }
}
