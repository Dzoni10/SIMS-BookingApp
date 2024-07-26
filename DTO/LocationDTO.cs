using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;
using BookingApp.Domain.Models;
using iText.Layout.Splitting;
namespace BookingApp.DTO
{
    public class LocationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string state;
        public string State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged("State");
                }
            }
        }
        private string city;
        public string City
        {
            get { return city; }

            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }
        private bool showCreateButton;
        public bool ShowCreateButton
        {
            get { return showCreateButton; }
            set
            {
                if(value != showCreateButton)
                {
                    showCreateButton = value;
                    OnPropertyChanged("ShowCreateButton");
                }
            }
        }

        private bool veryUsefulForum;
        public bool VeryUsefulForum
        {
            get { return veryUsefulForum; }
            set
            {
                if (veryUsefulForum != value)
                {
                    veryUsefulForum = value;
                    OnPropertyChanged("VeryUsefulForum");
                }
            }
        }

        public LocationDTO() { }
        public LocationDTO(int id, string state, string city, bool showCreateButton)
        {
            this.Id = id;
            this.state = state;
            this.city = city;
            this.showCreateButton = showCreateButton;
        }
        public LocationDTO(string state, string city, bool showCreateButton)
        {
            this.state = state;
            this.city = city;
            this.showCreateButton = showCreateButton;
        }
        public LocationDTO(int id, string state, string city, bool showCreateButton, bool veryUsefulForum)
        {
            this.Id = id;
            this.state = state;
            this.city = city;
            this.showCreateButton = showCreateButton;
            this.veryUsefulForum = veryUsefulForum;
        }
        public LocationDTO(string state, string city)
        {
            this.state = state;
            this.city = city;
        }
        public LocationDTO(Location location)
        {
            this.Id = location.Id;
            this.state = location.State;
            this.city = location.City;
        }

        public Location ToLocation()
        {
            return new Location(Id, state, city);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
