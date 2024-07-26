using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using iText.Commons.Utils;

namespace BookingApp.DTO
{
    public class GuestDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if(name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string surname;
        public string Surname
        {
            get { return surname;  }
            set
            {
                if(surname != value)
                {
                    surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if(username != value)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if(password != value)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        private bool superGuestStatus;
        public bool SuperGuestStatus
        {
            get { return superGuestStatus; }
            set
            {
                if (superGuestStatus != value)
                {
                    superGuestStatus = value;
                    OnPropertyChanged("SuperGuestStatus");
                }
            }
        }
        private int points;
        public int Points
        {
            get { return points; }
            set
            {
                if (points != value)
                {
                    points = value;
                    OnPropertyChanged("Points");
                }
            }
        }
        private DateTime superGuestExpirationDate;
        public DateTime SuperGuestExpirationDate
        {
            get { return superGuestExpirationDate; }
            set
            {
                if (superGuestExpirationDate != value)
                {
                    superGuestExpirationDate = value;
                    OnPropertyChanged("SuperGuestExpirationDate");
                }
            }
        }
        public GuestDTO() { }
        public GuestDTO(int id, string name, string surname,string username,string password, bool superGuestStatus, int points, DateTime superGuestExpirationDate)
        {
            this.Id = id;
            this.name = name;
            this.surname = surname;
            this.username = username;
            this.password = password;
            this.superGuestStatus = superGuestStatus;
            this.points = points;
            this.superGuestExpirationDate = superGuestExpirationDate;
        }
        public GuestDTO(string name, string surname,string password,string username, bool superGuestStatus, int points, DateTime superGuestExpirationDate)
        {
            this.name = name;
            this.surname = surname;
            this.username = username;
            this.password = password;
            this.superGuestStatus = superGuestStatus;
            this.points = points;
            this.superGuestExpirationDate = superGuestExpirationDate;
        }
        public GuestDTO(Guest guest)
        {
            this.name = guest.Name;
            this.surname = guest.Surname;
            this.username = guest.Username;
            this.password = guest.Password;
            this.superGuestStatus = guest.SuperGuestStatus;
            this.points = guest.Points;
            this.superGuestExpirationDate = guest.SuperGuestExpirationDate;
        }
        
        public Guest ToGuest()
        {
            return new Guest(Id, name, surname, superGuestStatus, points, superGuestExpirationDate);
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
