using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class OwnerDTO : User, INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string username;

        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
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
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                if (value != surname)
                {
                    surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }

        private bool isSuperOwner;
        public bool IsSuperOwner
        {
            get { return isSuperOwner; }
            set
            {
                if (value != isSuperOwner)
                {
                    isSuperOwner = value;
                    OnPropertyChanged("IsSuperOwner");
                }
            }
        }

        private double averageGrade;
        public double AverageGrade
        {
            get { return averageGrade; }
            set
            {
                if (value != averageGrade)
                {
                    averageGrade = value;
                    OnPropertyChanged("AverageGrade");
                }
            }
        }

        public OwnerDTO(string name, string surname)
        {
            this.name = name;
            this.surname = surname;
            AverageGrade = 0;
        }

        public OwnerDTO(string username, string password, string name, string surname, double averageGrade, bool isSuperOwner)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.averageGrade = averageGrade;
            this.isSuperOwner = isSuperOwner;
        }

        public OwnerDTO(int id, string username, string password, string name, string surname, double averageGrade, bool isSuperOwner)
        {
            this.Id = id;
            this.username = username;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.averageGrade = averageGrade;
            this.isSuperOwner = isSuperOwner;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
