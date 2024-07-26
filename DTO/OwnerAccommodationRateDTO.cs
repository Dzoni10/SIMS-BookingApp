using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Serializer;
using BookingApp.WPF.View;


namespace BookingApp.DTO
{
    public class OwnerAccommodationRateDTO : INotifyPropertyChanged
    {
        public int Id {  get; set; }

        private int accommodationReservationId;
        public int AccommodationReservationId
        {
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

        private int accommodationId;
        public int AccommodationId 
        {
          get { return accommodationId; }
          set 
            {
                if(accommodationId != value)
                {
                    accommodationId = value;
                    OnPropertyChanged("AccommodationId");
                }
            } 
        
        }
        private int guestId;

        public int GuestId
        {
            get
            { return guestId; }
            set
            {
                if(guestId != value)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }
        private double cleanlinessGrade;
        public double CleanlinessGrade 
        {
            get { return cleanlinessGrade; }
            set
            {
                if(cleanlinessGrade != value)
                {
                    cleanlinessGrade = value;
                    OnPropertyChanged("CleanlinessGrade");
                }
            }
        }
        private double ownerHospitality;
        public double OwnerHospitality 
        {
            get { return ownerHospitality; }
            set
            {
                if(ownerHospitality != value)
                {
                    ownerHospitality = value;
                    OnPropertyChanged("OwnerHospitality");
                }
            }
        }
        private string additionalComment;
        public string AdditionalComment 
        { 
            get { return additionalComment; }
            set
            { 
                if(additionalComment != value)
                { 
                    additionalComment= value;
                    OnPropertyChanged("AdditionalComment");
                }
            } 
        }

        private string renovationComment;
        public string RenovationComment 
        {
            get { return renovationComment; } 
            set
            {
                if(renovationComment != value)
                {
                    renovationComment= value;
                    OnPropertyChanged("RenovationComment");
                }
            }
        }
        private double renovationEmergencyGrade;
        public double RenovationEmergencyGrade 
        { 
            get { return renovationEmergencyGrade; } 
            
            set
            {
                if(renovationEmergencyGrade != value)
                {
                    renovationEmergencyGrade= value;
                    OnPropertyChanged("RenovationEmergencyGrade");
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
                    accommodation= value;
                    OnPropertyChanged("Accommmodation");
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
                if(guest != value)
                {
                    guest= value;
                    OnPropertyChanged("Guest");
                }
            }
        }
        public OwnerAccommodationRateDTO()
        {

        }
        public OwnerAccommodationRateDTO(int accommodationReservationId, int accommodationId,int guestId, double cleanlinessGrade, double ownerHospitality, string additionalComment, string renovationComment, double renovationEmergencyGrade)
        {
            this.accommodationReservationId = accommodationReservationId;
            this.accommodationId = accommodationId;
            this.guestId = guestId;
            this.cleanlinessGrade = cleanlinessGrade;
            this.ownerHospitality = ownerHospitality;
            this.additionalComment = additionalComment;
            this.renovationComment = renovationComment;
            this.renovationEmergencyGrade = renovationEmergencyGrade;
            accommodation = new Accommodation();
            guest = new Guest();
        }

        public OwnerAccommodationRateDTO(int id, int accommodationReservationId, int accommodationId,int guestId, double cleanlinessGrade, double ownerHospitality, string additionalComment, string renovationComment, double renovationEmergencyGrade)
        {
            this.Id = id;
            this.accommodationReservationId = accommodationReservationId;
            this.accommodationId = accommodationId;
            this.cleanlinessGrade = cleanlinessGrade;
            this.ownerHospitality = ownerHospitality;
            this.additionalComment = additionalComment;
            this.renovationComment = renovationComment;
            this.renovationEmergencyGrade = renovationEmergencyGrade;
        }

        public OwnerAccommodationRateDTO(OwnerAccommodationRate ownerAccommodationRate)
        {
            Id = ownerAccommodationRate.Id;
            accommodationReservationId = ownerAccommodationRate.AccommodationReservationId;
            accommodationId = ownerAccommodationRate.AccommodationId;
            guestId = ownerAccommodationRate.GuestId;
            cleanlinessGrade = ownerAccommodationRate.CleanlinessGrade;
            ownerHospitality = ownerAccommodationRate.OwnerHospitality;
            additionalComment = ownerAccommodationRate.AdditionalComment;
            accommodation = ownerAccommodationRate.Accommodation;
            guest = ownerAccommodationRate.Guest;
        }

        public OwnerAccommodationRate ToOwnerAccommodationRate()
        {
            return new OwnerAccommodationRate(Id, accommodationReservationId, accommodationId, guestId, cleanlinessGrade, ownerHospitality, additionalComment);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
