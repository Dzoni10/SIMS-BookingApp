using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class TourRequestDTO : INotifyPropertyChanged
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

        private User requestingUser;
        public User RequestingUser
        {
            get { return requestingUser; }
            set
            {
                if (value != requestingUser)
                {
                    requestingUser = value;
                    OnPropertyChanged("RequestingUser");
                }
            }
        }

        private TourRequestStatus status;
        public TourRequestStatus Status
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

        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private Language language; 
        public Language Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        private DateOnly dateFrom;
        public DateOnly DateFrom
        {
            get { return dateFrom; }
            set
            {
                if (value != dateFrom)
                {
                    dateFrom = value;
                    OnPropertyChanged("DateFrom");
                }
            }
        }

        private DateOnly dateTo;
        public DateOnly DateTo
        {
            get { return dateTo; }
            set
            {
                if (value != dateTo)
                {
                    dateTo = value;
                    OnPropertyChanged("DateTo");
                }
            }
        }

        private int capacity;
        public int Capacity
        {
            get { return capacity;}
            set
            {
                if(value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }

        private DateTime dateIfAccepted;

        public DateTime DateIfAccepted
        {
            get { return dateIfAccepted; }
            set
            {
                if(value != dateIfAccepted)
                {
                    dateIfAccepted = value;
                    OnPropertyChanged("DateIfAccepted");
                }
            }
        }
        private int complexTourRequestId;

        public int ComplexTourRequestId
        {
            get { return complexTourRequestId; }
            set
            {
                if (value != complexTourRequestId)
                {
                    complexTourRequestId = value;
                    OnPropertyChanged("ComplexTourRequestId");
                }
            }
        }

        public TourRequestDTO() { }

        public TourRequestDTO(TourRequest tourRequest)
        {
            Id = tourRequest.Id;
            RequestingUser = tourRequest.RequestingUser;
            Status = tourRequest.Status;
            Location = tourRequest.Location;
            Description = tourRequest.Description;
            Language = tourRequest.Language;
            DateFrom = tourRequest.DateFrom;
            DateTo = tourRequest.DateTo;
            Capacity = tourRequest.Capacity;
            DateIfAccepted = tourRequest.DateIfAccepted;
            ComplexTourRequestId = tourRequest.ComplexTourRequestId;
        }

        public TourRequest ToTourRequest()
        {
            return new TourRequest
            {
                Id = Id,
                RequestingUser = RequestingUser,
                Status = Status,
                Location = Location,
                Description = Description,
                Language = Language,
                DateFrom = DateFrom,
                DateTo = DateTo,
                Capacity = Capacity,
                DateIfAccepted = DateIfAccepted,
                ComplexTourRequestId = ComplexTourRequestId
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
