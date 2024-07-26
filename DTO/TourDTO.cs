using BookingApp.Application.Injector;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace BookingApp.DTO
{
    public class TourDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
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
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private Language language;
        public Language Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }
        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }
        private string capacitys;
        public string Capacitys
        {
            get { return capacitys; }
            set
            {
                if (value != capacitys)
                {
                    capacitys = value;
                    OnPropertyChanged("Capacitys");
                }
            }
        }
        private double duration;
        public double Duration
        {
            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        private StartTourDate startTourDate;
        public StartTourDate StartTourDate
        {
            get { return startTourDate; }
            set
            {
                if (value != startTourDate)
                {
                    startTourDate = value;
                    OnPropertyChanged("StartTourDate");
                }
            }
        }
        private int visitors;
        public int Visitors
        {
            get { return visitors; }
            set
            {
                if (value != visitors)
                {
                    visitors = value;
                    OnPropertyChanged("Visitors");
                }
            }
        }

        private string locationMade;

        public string LocationMade
        {
            get { return Location.City + ", " + Location.State; }
            set
            {
                if(value != locationMade)
                {
                    locationMade = value;
                    OnPropertyChanged("LocationMade");
                }
            }
        }

        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                if(value != path)
                {
                    path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        public string checkpoints;

        public string Checkpoints
        {
            get
            {
                CheckpointService service = new CheckpointService(Injector.CreateInstance<ICheckpointRepository>());
                checkpoints = "";
                foreach(Checkpoint checkpoint in service.GetAllByTourId(id))
                {
                    checkpoints += $"{checkpoint.Name},";
                }
                return checkpoints;
            }
            set
            {
                if (value != checkpoints)
                {
                    checkpoints = value;
                    OnPropertyChanged("Checkpoints");
                }
            }
        }

        private ObservableCollection<StartTourDateDTO> dates;
        public ObservableCollection<StartTourDateDTO> Dates
        {
            get
            {
                StartTourDateService service = new StartTourDateService(Injector.CreateInstance<IStartTourDateRepository>());
                List<StartTourDate> list = service.LoadTourDates(id);
                foreach(StartTourDate date in list)
                {
                    dates.Add(new StartTourDateDTO(date));
                }
                return dates;
            }
            set
            {
                if(value != dates)
                {
                    dates = value;
                    OnPropertyChanged("Dates");
                }
            }
        }
        public TourDTO()
        {
        }
        public TourDTO(Tour tour)
        {
            Id = tour.Id;
            Name = tour.Name;
            Location = tour.Location;
            Description = tour.Description;
            Language = tour.Language;
            Capacity = tour.Capacity;
            Duration = tour.Duration;
        }
        public Tour ToTour()
        {
            return new Tour
            {
                Id = Id,
                Name = Name,
                Location = Location,
                Description = Description,
                Language = Language,
                Capacity = Capacity,
                Duration = Duration
            };
        }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                        {
                            return "Please enter the value";
                        }
                        break;
                    case "Capacitys":
                        int r;
                        if (!int.TryParse(Capacitys, out r))
                        {
                            return "Please enter a int value.";
                        }
                        if (r < 1)
                        {
                            return "The value must be greather than 0";
                        }              
                        Capacity = r;                       
                        break;
                    case "Description":
                        if (string.IsNullOrEmpty(Description))
                        {
                            return "Please enter the value";
                        }
                        break;
                    case "Duration":
                        if (Duration <= 0)
                        {
                            return "The value must be positive number";
                        }
                        break;
                }
                return null;
            }
        }
        private readonly string[] _validateProperties = { "Name", "Description", "Duration", "Capacitys"};
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