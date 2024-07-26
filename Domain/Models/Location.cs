using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Models
{
    public class Location : ISerializable,IDataErrorInfo,INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string state;
        public string State
        {
            get { return state; }
            set
            {
                if(state != value)
                {
                    state = value;
                    OnPropertyChanged();
                }
            }
        }
        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if(city!= value)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }
        public Location() { }
        public Location(int id, string state, string city)
        {
            Id = id;
            State = state;
            City = city;

        }
        public Location(string state, string city)
        {
            State = state;
            City = city;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            State = values[1];
            City = values[2];
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                State,
                City,

            };
            return csvValues;
        }
        public override string ToString()
        {
            return State + ", " + City;
        }
        public string Error => null;
        public string this[string columnName]{
            get{
                string pattern = @"\d";
                Regex regex = new Regex(pattern);
                switch (columnName){ 
                    case "State":
                        if (string.IsNullOrWhiteSpace(State))
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "The field must be filled!" : "Polje mora biti popunjeno";
                        if (regex.IsMatch(State)){
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "Field supprot only string" : "Polje podržava samo tekst";
                        }break;
                    case "City":
                        if (string.IsNullOrWhiteSpace(City))
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "The field must be filled!":"Polje mora biti popunjeno";
                        if (regex.IsMatch(City)) {
                            return (Properties.Settings.Default.currentLanguage == "en-US") ? "Field support only string" :"Polje podržava samo tekst";
                        }break;
                }
                return null;
            }
        }
        private readonly string[] _validateProperties = { "State", "City" };
        public bool IsValid{
            get{
                foreach (var property in _validateProperties){
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
