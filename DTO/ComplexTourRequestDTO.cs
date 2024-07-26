using BookingApp.Domain.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class ComplexTourRequestDTO : INotifyPropertyChanged
    {
        private int id;
        private int capacity;
        private int touristId;
        private ComplexTourRequestStatus status;
        private DateOnly startingDate;

        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TouristId
        {
            get { return touristId; }
            set
            {
                if(value != touristId)
                {
                    touristId = value;
                    OnPropertyChanged("TouristId");
                }
            }
        }

        public ComplexTourRequestStatus Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly StartingDate
        {
            get { return startingDate; }
            set
            {
                if (value != startingDate)
                {
                    startingDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public ComplexTourRequestDTO()
        {
        }

        public ComplexTourRequestDTO(ComplexTourRequest request)
        {
            Id = request.Id;
            Capacity = request.Capacity;
            TouristId = request.TouristId;
            Status = request.Status;
            StartingDate = request.StartingDate;
        }

        public ComplexTourRequest ToComplexTourRequest()
        {
            return new ComplexTourRequest
            {
                Id = Id,
                Capacity = Capacity,
                TouristId = TouristId,
                Status = Status,
                StartingDate = StartingDate
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
