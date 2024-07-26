using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Serializer;

namespace BookingApp.DTO
{
    public class AccommodationReservationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if (value != accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged("AccommodationId");
                }
            }
        }

        private Accommodation accommodation;
        public Accommodation Accommodation
        {
            get { return accommodation; }
            set
            {
                if (value != accommodation)
                {
                    accommodation = value;
                    OnPropertyChanged("Accommodation");
                }
            }
        }
        private AccommodationType type;
        public AccommodationType Type
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

        private int locationId;
        public int LocationId
        {
            get { return locationId; }
            set
            {
                if (value != locationId)
                {
                    locationId = value;
                    OnPropertyChanged("LocationId");
                }
            }
        }

        private Location location;
        public Location Location
        {
            get
            {
                return location;
            }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }

        private int guestCount;

        public int GuestCount
        {
            get { return guestCount; }
            set
            {
                if (value != guestCount)
                {
                    guestCount = value;
                    OnPropertyChanged("GuestCount");
                }
            }
        }

        private int daysReserved;
        public int DaysReserved
        {
            get { return daysReserved; }
            set
            {
                if (value != daysReserved)
                {
                    daysReserved = value;
                    OnPropertyChanged("DaysReserved");
                }
            }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        private int guestId;
        public int GuestId
        {
            get { return guestId; }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }

        private Guest guest;
        public Guest Guest 
        {
            get
            {
                return guest;
            }
            set
            {
                if(value != guest)
                {
                    guest = value;
                    OnPropertyChanged("Guest");
                }
            }
        }

        private string accommodationName;
        public string AccommodationName
        {
            get
            {
                return accommodationName;
            }
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged("AccommodationName");
                }
            }
        }
        private RateStatus ownerRateStatus;
        public RateStatus OwnerRateStatus
        {
            get
            {
                return ownerRateStatus;
            }
            set
            {
                if (value != ownerRateStatus)
                {
                    ownerRateStatus = value;
                    OnPropertyChanged("OwnerRateStatus");
                }
            }
        }
        private RateStatus guestRateStatus;
        public RateStatus GuestRateStatus
        {
            get
            {
                return guestRateStatus;
            }
            set
            {
                if (value != guestRateStatus)
                {
                    guestRateStatus = value;
                    OnPropertyChanged("GuestRateStatus");
                }
            }
        }
        private string rateDaysLeft;
        public string RateDaysLeft
        {
            get
            {
                return rateDaysLeft;
            }
            set
            {
                if(value != rateDaysLeft)
                {
                    rateDaysLeft = value;
                    OnPropertyChanged("RateDaysLeft");
                }
            }
        }
        private Image image;
        public Image Image
        {
            get { return image; }
            set
            {
                if (value != image)
                {
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }
        private bool ratedAccommodation;
        public bool RatedAccommodation
        {
            get { return ratedAccommodation; }
            set
            {
                if (value != ratedAccommodation)
                {
                    ratedAccommodation = value;
                    OnPropertyChanged("RatedAccommodation");
                }
            }
        }
        private bool possibleCancellation;
        public bool PossibleCancellation
        {
            get { return possibleCancellation; }
            set
            {
                if (value != possibleCancellation)
                {
                    possibleCancellation = value;
                    OnPropertyChanged("PossibleCancellation");
                }
            }
        }
        public AccommodationReservationDTO() { }
        public AccommodationReservationDTO(int accommodationId, int locationId, int guestId, int guestCount, int daysReserved, DateTime startDate, DateTime endDate,RateStatus guestRateStatus, RateStatus ownerRateStatus)
        {
            this.accommodationId = accommodationId;
            this.locationId = locationId;
            this.guestId = guestId;
            this.guestCount = guestCount;
            this.daysReserved = daysReserved;
            this.startDate = startDate;
            this.endDate = endDate;
            this.guestRateStatus = guestRateStatus;
            this.ownerRateStatus = ownerRateStatus;
        }
        public AccommodationReservationDTO(int id, int accommodationId, int locationId, int guestId, int guestCount, int daysReserved, DateTime startDate, DateTime endDate, RateStatus guestRateStatus, RateStatus ownerRateStatus)
        {
            this.Id = id;
            this.accommodationId = accommodationId;
            this.locationId = locationId;
            this.guestId = guestId;
            this.guestCount = guestCount;
            this.daysReserved = daysReserved;
            this.startDate = startDate;
            this.endDate = endDate;
            this.guestRateStatus = guestRateStatus;
            this.ownerRateStatus = ownerRateStatus;
            this.guest = new Guest();
            this.location = new Location();
            this.accommodation = new Accommodation();
        }
        public AccommodationReservationDTO(AccommodationReservation reservation)
        {
            Id = reservation.Id;
            accommodationId = reservation.AccommodationId;
            locationId = reservation.LocationId;
            guestId = reservation.GuestId;
            guestCount = reservation.GuestCount;
            daysReserved = reservation.DaysReserved;
            startDate = reservation.StartDate;
            endDate = reservation.EndDate;
            guest = reservation.Guest;
            accommodation = reservation.Accommodation;
            guestRateStatus = reservation.GuestRateStatus;
            ownerRateStatus = reservation.OwnerRateStatus;
            guest = reservation.Guest;
            location = new Location();
            accommodation = reservation.Accommodation;
        }
        public AccommodationReservation ToAccommodationReservation()
        {
            return new AccommodationReservation(Id, accommodationId, locationId, guestId, guestCount, daysReserved, startDate, endDate, guestRateStatus, ownerRateStatus);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

