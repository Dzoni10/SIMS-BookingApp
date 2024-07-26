using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IStartTourDateRepository
    {
        public StartTourDate Save(StartTourDate startTourDate);
        public void SaveAll(List<StartTourDate> startTourDates);
        public void Update(StartTourDate startTourDate);
        public StartTourDate GetById(int id);
        public List<StartTourDate> GetAll();
    }
}
