using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using BookingApp.Domain.Models;
namespace BookingApp.DTO
{
    public class AccommodationDTO : INotifyPropertyChanged,IDataErrorInfo
    {
            public int Id { get; set; }
            private string name;
            public string Name{
                get { return name; }
                set{
                    if (value != name){
                        name = value;
                        OnPropertyChanged("Name");
                    }
                }
            }
            private Location location;
            public Location Location{
                get { return location; }
                set{
                    if (value != location){
                        location = value;
                        OnPropertyChanged("Location");
                    }
                }
            }
            private AccommodationType type;
            public AccommodationType Type{
                get{
                    return type;    
                }
                set{
                    if(value != type){
                        type = value;
                        OnPropertyChanged("Type");
                    }
                }
            }
            private int capacity;
            public int Capacity{
                get { return capacity; }
                set{
                    if (value != capacity){
                        capacity = value;
                        OnPropertyChanged("Capacity");
                    }
                }
            }
       private int minStayDays;
       public int MinStayDays{
           get { return minStayDays; }
           set{
                if (value != minStayDays){
                    minStayDays = value;
                    OnPropertyChanged("MinStayDays");
                    }
                }
            }
       private int cancelationPeriod;
       public int CancelationPeriod{
           get { return cancelationPeriod; }
           set{
                if (value != cancelationPeriod){
                    cancelationPeriod = value;
                    OnPropertyChanged("CancelationPeriod");
                    }
                }
            }
        private int locationId;
        public int LocationId{
            get { return locationId; }
            set{
                if(value != locationId){
                    locationId = value;
                    OnPropertyChanged("LocationId");
                }
            }
        }
        private Image image;
        public Image Image{
            get { return image; }
            set{
                if (value != image){
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }
        private string averageRate;
        public string AverageRate
        {
            get { return averageRate; }
            set
            {
                if (value != averageRate)
                {
                    averageRate = value;
                    OnPropertyChanged("AverageRate");
                }
            }
        }

        private bool isClosed;
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if(value!=isClosed)
                {
                    isClosed = value;
                    OnPropertyChanged("IsClosed");
                }
            }
        }
        public AccommodationDTO() { }
            public Accommodation ToAccommodation(){
                return new Accommodation(Id, name, type, capacity, minStayDays, cancelationPeriod,locationId,isClosed);
            }
            public AccommodationDTO(string name, AccommodationType type, int capacity, int minStayDays, int cancelationPeriod,int locationId,bool isClosed){
                this.name = name;
                this.type = type;
                this.capacity = capacity;
                this.minStayDays = minStayDays;
                this.cancelationPeriod = cancelationPeriod;
                this.locationId = locationId;
                this.isClosed = isClosed;
                location = new Location();
                image = new Image();
            }
            public AccommodationDTO(int id, string name, AccommodationType type, int capacity, int minStayDays, int cancelationPeriod,int locationId,bool isClosed){
                this.Id = id;
                this.name = name;
                this.type = type;
                this.capacity = capacity;
                this.minStayDays = minStayDays;
                this.cancelationPeriod = cancelationPeriod;
                this.locationId = locationId;
                this.isClosed = isClosed;
            }
            public AccommodationDTO(Accommodation accommodation){
                Id = accommodation.Id;
                name = accommodation.Name;
                type = accommodation.Type;
                capacity = accommodation.Capacity;
                minStayDays = accommodation.MinStayDays;
                cancelationPeriod = accommodation.CancelationPeriod;
                location = accommodation.Location;
                locationId = accommodation.LocationId;
                isClosed = accommodation.IsClosed;
                image = accommodation.Image;
            }
        public string Error => null;
        public string this[string columnName]{
            get{
                string pattern = @"^(?=.*[a-zA-Z])[\w\s]*$";
                Regex regex = new Regex(pattern);
                switch (columnName){
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "The field must be filled" :"Polje mora biti popunjeno";
                        if (!regex.IsMatch(Name))
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "Field not support only numbers":"Ne možete uneti samo brojeve";
                        break;
                    case "Capacity":
                        if (Capacity<1){
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "The value must be greather than 0" : "Vrednost mora biti veća od 0";
                        }break;
                    case "MinStayDays":
                        if (MinStayDays < 1){
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "The value must be greather than 0": "Vrednost mora biti veća od 0";
                        }break;
                    case "CancelationPeriod":
                        if (CancelationPeriod < 1){
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "The value must be greather than 0" : "Vrednost mora biti veća od 0";
                        }
                        break;
                    case "Type":
                        if (Type == 0)
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "Type must be selected" :"Morate izabrati tip";
                        break;
                }
                return null;
            }
        }
        private readonly string[] _validateProperties = { "Name", "CancelationPeriod", "MinStayDays", "Capacity","Type"};
        public bool IsValid{
            get{
                foreach(var property in _validateProperties){
                    if (this[property] != null)
                        return false;
                }
                return true;
            }
        }
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null){
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
}
