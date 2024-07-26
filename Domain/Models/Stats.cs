using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum Month {Jan=1,Feb,Mar,Apr,May,Jun,Jul,Avg,Sep,Oct,Nov,Dec };

namespace BookingApp.Domain.Models
{
    public class Stats
    {
        public int Year { get; set; }
        public Month Month { get; set; }
        public int Reserved { get; set; }
        public int Canceled { get; set; }
        public int Rescheduled { get; set; }
        public int Advices { get; set; }

        public Stats() { }
        public Stats(int year, int reserved, int canceled, int rescheduled, int advices)
        {
            Year = year;
            Reserved = reserved;
            Canceled = canceled;
            Rescheduled = rescheduled;
            Advices = advices;
        }

        public Stats(Month month, int reserved, int canceled, int rescheduled, int advices)
        {
            Month = month;
            Reserved = reserved;
            Canceled = canceled;
            Rescheduled = rescheduled;
            Advices = advices;
        }
    }
}
