using BookingApp.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class TourGuestDTO : INotifyPropertyChanged
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

        private int tourReservationId;
        public int TourReservationId
        {
            get { return tourReservationId; }
            set
            {
                if (value != tourReservationId)
                {
                    tourReservationId = value;
                    OnPropertyChanged("TourReservationId");
                }
            }
        }

        private int checkPointId;
        public int CheckPointId
        {
            get { return checkPointId; }
            set
            {
                if (value != checkPointId)
                {
                    checkPointId = value;
                    OnPropertyChanged("CheckPointId");
                }
            }
        }

        private bool @checked;
        public bool Checked
        {
            get { return @checked; }
            set
            {
                if (@checked != value)
                {
                    @checked = value;
                    OnPropertyChanged("Checked");
                }
            }
        }


        public TourGuestDTO()
        {
        }

        public TourGuestDTO(TourGuest tourGuest)
        {
            Id = tourGuest.Id;
            FullName = tourGuest.FullName;
            Age = tourGuest.Age;
            TourReservationId = tourGuest.TourReservationId;
            CheckPointId = tourGuest.CheckPointId;
        }

        public TourGuest ToTourGuest()
        {
            return new TourGuest
            {
                Id = Id,
                FullName = FullName,
                Age = Age,
                TourReservationId = TourReservationId,
                CheckPointId = CheckPointId
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }   
    }
}
