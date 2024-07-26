using BookingApp.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class StartTourDateDTO : INotifyPropertyChanged
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

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private int tourId;
        public int TourId
        {
            get { return tourId; }
            set
            {
                if (value != tourId)
                {
                    tourId = value;
                    OnPropertyChanged("TourId");
                }
            }
        }

        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }

        private TourStatus status;
        public TourStatus Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private int currentCheckpoint;
        public int CurrentCheckpoint
        {
            get { return currentCheckpoint; }
            set
            {
                if (value != currentCheckpoint)
                {
                    currentCheckpoint = value;
                    OnPropertyChanged("CurrentCheckpoint");
                }
            }
        }

        public StartTourDateDTO()
        {
        }

        public StartTourDateDTO(StartTourDate reservation)
        {
            Id = reservation.Id;
            Date = reservation.Date;
            TourId = reservation.TourId;
            Capacity = reservation.Capacity;
            Status = reservation.Status;
            CurrentCheckpoint = reservation.CurrentCheckpoint;
        }

        public StartTourDate ToStartTourDate()
        {
            return new StartTourDate
            {
                Id = Id,
                Date = Date,
                TourId = TourId,
                Capacity = Capacity,
                Status = Status,
                CurrentCheckpoint = CurrentCheckpoint
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
