using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class RenovationDTO : INotifyPropertyChanged,IDataErrorInfo
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
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if(startDate != value)
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
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        private Accommodation accommodation;
        public Accommodation Accommodation
        {
            get { return accommodation; }
            set
            {
                if(accommodation != value)
                {
                    accommodation = value;
                    OnPropertyChanged("Accommodation");
                }
            }
        }
        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                if(duration != value)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if(description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public RenovationDTO() { }
        public Renovation ToRenovation()
        {
            return new Renovation(Id, accommodationId, startDate, endDate, duration, description);
        }
        public RenovationDTO(int accommodationId,DateTime startDate,DateTime endDate,int duration,string description)
        {
            this.accommodationId=accommodationId;
            this.startDate=startDate;
            this.endDate=endDate;
            this.duration = duration;
            this.description = description;
            accommodation = new Accommodation();
        }
        public RenovationDTO(int id,int accommodationId,DateTime startDate, DateTime endDate,int duration,string description)
        {
            Id = id;
            this.accommodationId = accommodationId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.duration = duration;
            this.description = description;
        }
        public RenovationDTO(Renovation renovation)
        {
            Id= renovation.Id;
            accommodationId=renovation.AccommodationId;
            startDate= renovation.StartDate;
            endDate= renovation.EndDate;
            duration = renovation.Duration;
            description = renovation.Description;
            accommodation = renovation.Accommodation;
        }
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case "Duration":
                        if (Duration<1 || (EndDate - StartDate).TotalDays <Duration)
                        {
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "Can't be 0 and greater than date range" : "Ne moze biti 0 i vece od opsega datuma";
                        }
                        break;
                    case "Description":
                        if(string.IsNullOrEmpty(Description))
                        {
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "The field must be filled" : "Polje mora biti popunjeno";
                        }
                        break;
                }
                return null;
            }
        }
        private readonly string[] _validateProperties = { "Duration", "Description" };
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
