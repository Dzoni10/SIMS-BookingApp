using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;

namespace BookingApp.DTO
{
    public class IgnoreDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if (accommodationId == value)
                    accommodationId = value;
                OnPropertyChanged("AccommodationId");
            }
        }

        private int guestId;
        public int GuestId
        {
            get { return guestId; }
            set
            {
                if (guestId == value)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }
        private int editedReservationId;
        public int EditedReservationId
        {
            get { return editedReservationId; }
            set
            {
                if(editedReservationId == value)
                {
                    editedReservationId = value;
                    OnPropertyChanged("EditedGuestId");
                }
            }
        }

        private string explanation;
        public string Explanation
        {
            get { return explanation; }
            set
            {
                if(explanation == value)
                {
                    explanation = value;
                    OnPropertyChanged("Explanation");
                }
            }
        }

        public IgnoreDTO() { }
        public IgnoreDTO(int accommodationId,int guestId,int editedReservationId,string explanation) 
        {
            this.accommodationId = accommodationId;
            this.guestId = guestId;
            this.editedReservationId = editedReservationId;
            this.explanation = explanation;
        }

        public IgnoreDTO(int id,int accommodationId, int guestId, int editedReservationId, string explanation)
        {
            Id = id;
            this.accommodationId = accommodationId;
            this.guestId = guestId;
            this.editedReservationId = editedReservationId;
            this.explanation = explanation;
        }

        public IgnoreDTO(Ignore ignore)
        {
            Id = ignore.Id;
            accommodationId = ignore.AccommodationId;
            guestId = ignore.GuestId;
            editedReservationId = ignore.EditedReservationId;
            explanation = ignore.Explanation;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}