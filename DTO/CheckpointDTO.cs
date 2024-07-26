using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using BookingApp.Model;

namespace BookingApp.DTO
{
    public class CheckpointDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private int tourID;
        public int TourId 
        {
            get { return tourID; }
            set
            {
                if (tourID != value)
                {
                    tourID = value;
                    OnPropertyChanged("TourID");
                }
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged("Checked");
                }
            }
        }

        public CheckpointDTO() { }

        public CheckpointDTO(Checkpoint checkpoint)
        {
            Id = checkpoint.Id;
            Name = checkpoint.Name;
            TourId = checkpoint.TourId;
        }


        public Checkpoint ToCheckpoint()
        {
            return new Checkpoint(Name, TourId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
