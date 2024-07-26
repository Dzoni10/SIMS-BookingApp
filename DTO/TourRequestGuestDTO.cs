using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourRequestGuestDTO : INotifyPropertyChanged
    {
        private int id;
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

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (value != fullName)
                {
                    fullName = value;
                    OnPropertyChanged("FullName");
                }
            }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value != age)
                {
                    age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        private int tourRequestReservationId;
        public int TourRequestReservationId
        {
            get { return tourRequestReservationId; }
            set
            {
                if (value != tourRequestReservationId)
                {
                    tourRequestReservationId = value;
                    OnPropertyChanged("TourRequestReservationId");
                }
            }
        }


        public TourRequestGuestDTO()
        {
        }

        public TourRequestGuestDTO(TourRequestGuest tourGuest)
        {
            Id = tourGuest.Id;
            FullName = tourGuest.FullName;
            Age = tourGuest.Age;
            TourRequestReservationId = tourGuest.TourRequestReservationId;
        }

        public TourRequestGuest ToTourRequestGuest()
        {
            return new TourRequestGuest
            {
                Id = Id,
                FullName = FullName,
                Age = Age,
                TourRequestReservationId = TourRequestReservationId
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
