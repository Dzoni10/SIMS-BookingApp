using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class NotificationDTO : INotifyPropertyChanged
    {
        private int id;
        private int userId;
        private string text;

        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public int UserId
        {
            get { return userId; }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value != text)
                {
                    text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public NotificationType type;
        public NotificationType Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public NotificationDTO()
        {
        }

        public NotificationDTO(Notification notification)
        {
            Id = notification.Id;
            UserId = notification.UserId;
            Type = notification.Type;
            Text = notification.Text;
        }

        public Notification ToNotification()
        {
            return new Notification
            {
                Id = Id,
                UserId = UserId,
                Type = Type,
                Text = Text
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
