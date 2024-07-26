using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class StatsDTO : INotifyPropertyChanged
    {
        private int year;
        private Month month;
        public Month Month
        {
            get { return month; }
            set
            {
                if(month != value)
                {
                    month = value;
                    OnPropertyChanged("Month");
                }
            }
        }
        public int Year
        {
            get { return year; }
            set
            {
                if(year != value)
                    year = value;
                    OnPropertyChanged("Year");
            }
        }
        private int reserved;
        public int Reserved
        {
            get { return reserved; }
            set
            {
                if(reserved != value)
                {
                    reserved = value;
                    OnPropertyChanged("Reserved");
                }
            }
        }

        private int canceled;
        public int Canceled
        {
            get { return canceled;}
            set
            {
                if(canceled != value)
                {
                    canceled = value;
                    OnPropertyChanged("Canceled");
                }
            }
        }
        private int rescheduled;
        public int Rescheduled
        {
            get { return rescheduled; }
            set
            {
                if (rescheduled != value)
                {
                    rescheduled = value;
                    OnPropertyChanged("Rescheduled");
                }
            }
        }
        private int advices;
        public int Advices
        {
            get { return advices; }
            set
            {
                if(advices != value)
                {
                    advices = value;
                    OnPropertyChanged("Advices");
                }
            }
        }

        public StatsDTO() { }
        
        public StatsDTO(int year,int reserved,int canceled,int rescheduled,int advices)
        {
            this.year = year;
            this.reserved = reserved;
            this.canceled = canceled;
            this.rescheduled = rescheduled;
            this.advices = advices;
        }

        public StatsDTO(Month month,int reserved,int canceled,int rescheduled,int advices)
        {
            this.month = month;
            this.reserved=reserved;
            this.canceled=canceled;
            this.rescheduled=rescheduled;
            this.advices=advices;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
