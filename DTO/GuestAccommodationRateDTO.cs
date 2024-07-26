using BookingApp.Domain.Models;
using LiveCharts.Definitions.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class GuestAccommodationRateDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }

        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if (accommodationId != value)
                {
                    accommodationId = value;
                    OnPropertyChanged("AccommodationId");
                }
            }

        }
        private int accommodationReservationId;
        public int AccommodationReservationId {
            
            get { return accommodationReservationId; }
            set
            {
                if(accommodationReservationId != value)
                {
                    accommodationReservationId = value;
                    OnPropertyChanged("AccommodationReservationId");
                }
            }
        }

        private double cleanlinessGrade;
        public double CleanlinessGrade
        {
            get { return cleanlinessGrade; }
            set
            {
                if (cleanlinessGrade != value)
                {
                    cleanlinessGrade = value;
                    OnPropertyChanged("CleanlinessGrade");
                }
            }
        }

        private double ruleGrade;
        public double RuleGrade
        {
            get { return ruleGrade; }
            set
            {
                if (ruleGrade != value)
                {
                    ruleGrade = value;
                    OnPropertyChanged("RuleGrade");
                }
            }
        }

        private int guestId;
        public int GuestId
        {
            get
            {
                return guestId;

            }
            set
            {
                if(guestId != value)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
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

        public GuestAccommodationRateDTO() { }

        public GuestAccommodationRate ToGuestAccommodationRate() 
        {
            return new GuestAccommodationRate(Id,accommodationId,guestId,accommodationReservationId,cleanlinessGrade,ruleGrade,comment);    
        }

        public GuestAccommodationRateDTO(int accommodationId,int accommodationReservationId,double cleanlinessGrade,double ruleGrade,string comment,int guestId)
        {
            this.accommodationId = accommodationId;
            this.accommodationReservationId = accommodationReservationId;
            this.cleanlinessGrade = cleanlinessGrade;
            this.ruleGrade = ruleGrade;
            this.comment = comment;
            this.guestId=guestId;
        }


        public GuestAccommodationRateDTO(int id,int accommodationId,int accommodationReservationId, double cleanlinessGrade, double ruleGrade, string comment,int guestId)
        {
            this.Id = id;
            this.accommodationId = accommodationId;
            this.accommodationReservationId = accommodationReservationId;
            this.cleanlinessGrade = cleanlinessGrade;
            this.ruleGrade = ruleGrade;
            this.comment = comment;
            this.guestId = guestId;
        }

        public GuestAccommodationRateDTO(GuestAccommodationRate guestAccommodationRate)
        {
            Id = guestAccommodationRate.Id;
            accommodationId = guestAccommodationRate.AccommodationId;
            accommodationReservationId = guestAccommodationRate.AccommodationReservationId;
            cleanlinessGrade = guestAccommodationRate.CleanlinessGrade;
            ruleGrade = guestAccommodationRate.RuleGrade;
            comment = guestAccommodationRate.Comment;
            guestId = guestAccommodationRate.GuestId;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName) {
                    case "CleanlinessGrade":
                        if (CleanlinessGrade == 0)
                        {
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "You must choose grade" : "Moraš izabrati ocenu";
                        }
                        break;
                    case "RuleGrade":
                        if (RuleGrade == 0)
                        {
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "You must choose grade" : "Moraš izabrati ocenu";
                        }
                        break;
                }
                    return null;
            }
        }

        private readonly string[] _validateProperties = { "CleanlinessGrade", "RuleGrade" };
        public bool IsValid
        {
            get
            {
                foreach (var property in _validateProperties)
                {
                    if (this[property] != null)
                        return false;
                }
                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
