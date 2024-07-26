using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class TourReservationDTO : INotifyPropertyChanged
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

        private User user;
        public User User
        {
            get { return user; }
            set
            {
                if (value != user)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        private int startTourDateId;
        public int StartTourDateId
        {
            get { return startTourDateId; }
            set
            {
                if (value != startTourDateId)
                {
                    startTourDateId = value;
                    OnPropertyChanged("StartTourDateId");
                }
            }
        }

        public TourReservationDTO()
        {
        }

        public TourReservationDTO(TourReservation tourReservation)
        {
            Id = tourReservation.Id;
            User = tourReservation.User;
            StartTourDateId = tourReservation.StartTourDateId;
        }

        public TourReservation ToTourReservation()
        {
            return new TourReservation
            {
                Id = Id,
                User = User,
                StartTourDateId = StartTourDateId
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
