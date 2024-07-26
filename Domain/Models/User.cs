using BookingApp.Serializer;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public enum USERROLE { Owner, Guide, Guest, Tourist }

namespace BookingApp.Domain.Models
{
    public class User : ISerializable, INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }
        private USERROLE role;

        public USERROLE Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged();
            }
        }

        private int reservationCount;

        public int ReservationCount
        {
            get { return reservationCount; }
            set
            {
                if (reservationCount != value)
                {
                    reservationCount = value;
                    OnPropertyChanged("ReservationCount");
                }
            }
        }

        private DateTime firstReservationDate;

        public DateTime FirstReservationDate
        {
            get { return firstReservationDate; }
            set
            {
                if(firstReservationDate != value)
                {
                    firstReservationDate = value;
                    OnPropertyChanged("FirstReservationDate");
                }
            }
        }

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            ReservationCount = 0;
            FirstReservationDate = DateTime.Now;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Username,
                Password,
                Role.ToString(),
                ReservationCount.ToString(),
                FirstReservationDate.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            username = values[1];
            password = values[2];
            if (string.Equals(values[3], "Owner"))
                role = USERROLE.Owner;
            else if (string.Equals(values[3], "Guest"))
                role = USERROLE.Guest;
            else if (string.Equals(values[3], "Tourist"))
                role = USERROLE.Tourist;
            else
                role = USERROLE.Guide;
            reservationCount = int.Parse(values[4]);
            firstReservationDate = Convert.ToDateTime(values[5]);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
