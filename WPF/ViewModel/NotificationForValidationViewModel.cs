using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel
{
    public class NotificationForValidationViewModel : INotifyPropertyChanged
    {
        private string title = string.Empty;
        private string message = string.Empty;
        private string okText = "OK";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get => this.title;
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        public string Message
        {
            get => this.message;
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }
        }

        public string OkText
        {
            get => this.okText;
            set
            {
                if (this.okText != value)
                {
                    this.okText = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OkText"));
                }
            }
        }
    }
}
