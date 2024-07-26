using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
namespace BookingApp.DTO
{   public class EditedReservationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int accommodationReservationId;
        public int AccommodationReservationId{
            get { return accommodationReservationId; }
            set{
                if(accommodationReservationId != value){
                    accommodationReservationId = value;
                    OnPropertyChanged("AccommodationId");
                }
            }
        }
        private int accommodationId;
        public int AccommodationId{
            get { return accommodationId; }
            set{
                if(accommodationId != value){
                    accommodationId = value;
                    OnPropertyChanged("AccommodationId");
                }
            }
        }
        private int guestId;
        public int GuestId{
            get { return guestId; }
            set{
                if (guestId != value){
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }
     
        private DateTime startDate;
        public DateTime StartDate {
            get { return startDate; }
            set
            { 
                if(startDate != value){
                    startDate = value;
                    OnPropertyChanged("StartDate");
                }
            } 
        }
        private DateTime endDate;
        public DateTime EndDate{
            get { return endDate; }
            set
            {
                if(endDate != value){
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        private ReservationStatus status;
        public ReservationStatus Status{   
            get { return status; }
            set{ 
                if(status != value){
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        private Accommodation accommodation;
        public Accommodation Accommodation{
            get { return accommodation; }
            set{
                if(accommodation != value){
                    accommodation = value;
                    OnPropertyChanged("Accommodation");
                }
            }
        }
        private Guest guest;
        public Guest Guest{
            get { return guest; }
            set{
                if(guest != value){
                    guest = value;
                    OnPropertyChanged("Guest");
                }
            }
        }
        private string ownerComment;
        public string OwnerComment{
            get { return ownerComment; }
            set{
                if (ownerComment != value){
                    ownerComment = value;
                    OnPropertyChanged("OwnerComment");
                }
            }
        }
        private string accommodationName;
        public string AccommodationName{
            get { return accommodationName; }
            set{
                if (accommodationName != value){
                    accommodationName = value;
                    OnPropertyChanged("AccommodationName");
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
        public EditedReservationDTO() { }
        public EditedReservationDTO(int accommodationReservationId,int accommodationId,int guestId, DateTime newStartDate, DateTime newEndDate, ReservationStatus status){
            this.accommodationReservationId = accommodationReservationId;
            this.accommodationId = accommodationId;
            this.guestId = guestId;
            this.startDate = newStartDate;
            this.endDate = newEndDate;
            this.status = status;
            accommodation = new Accommodation();
            guest = new Guest();
        }
        public EditedReservationDTO(int id, int accommodationReservationId,int accommodationId,int guestId, DateTime newStartDate, DateTime newEndDate, ReservationStatus status){
            Id = id;
            this.accommodationReservationId = accommodationReservationId;
            this.accommodationId = accommodationId;
            this.guestId = guestId;
            this.startDate = newStartDate;
            this.endDate = newEndDate;
            this.status = status;
        }
        public EditedReservationDTO(EditedReservation editedReservation){
            Id = editedReservation.Id;
            accommodationReservationId = editedReservation.AccommodationReservationId;
            accommodationId=editedReservation.AccommodationId ;
            guestId = editedReservation.GuestId ;
            startDate = editedReservation.StartDate;
            endDate = editedReservation.EndDate;
            status = editedReservation.Status;
            accommodation = editedReservation.Accommodation;
            guest = editedReservation.Guest;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }
}
