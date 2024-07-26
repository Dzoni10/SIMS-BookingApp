using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class OwnerGuestNotificationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int editedReservationId;
        public int EditedReservationId
        {
            get { return editedReservationId; }
            set
            {
                if (editedReservationId != value)
                {
                    editedReservationId = value;
                    OnPropertyChanged("EditedReservationId");
                }
            }
        }
        private ReservationStatus status;
        public ReservationStatus Status
        {
            get { return status; }
            set
            {
                if(status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public OwnerGuestNotificationDTO() { }

        public OwnerGuestNotificationDTO(int editedReservationId,ReservationStatus status)
        {
            this.editedReservationId = editedReservationId;
            this.status = status;
        }

        public OwnerGuestNotificationDTO(int id,int editedReservationId,ReservationStatus status)
        {
            Id = id;
            this.editedReservationId = editedReservationId;
            this.status = status;
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
