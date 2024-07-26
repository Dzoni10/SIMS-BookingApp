using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourRequestStatisticsDTO : INotifyPropertyChanged
    {
        private string year;
        public string Year 
        {
            get { return year; }
            set
            {
                if(value != year)
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        private int requestNumber;
        public int RequestNumber
        {
            get { return requestNumber; }
            set
            {
                if (value != requestNumber)
                {
                    requestNumber = value;
                    OnPropertyChanged("RequestNumber");
                }
            }
        }

        public TourRequestStatisticsDTO()
        {

        }
        public TourRequestStatisticsDTO(string year, int requestNumber)
        {
            Year = year;
            RequestNumber = requestNumber;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
