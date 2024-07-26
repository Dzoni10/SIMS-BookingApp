using BookingApp.Domain.Models;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuideRepository
    {
        public Guide Save(Guide guide);
        public void SaveAll(List<Guide> guides);
        public void Update(Guide guide);
        public Guide GetById(int id);
        public List<Guide> GetAll();
    }
}
