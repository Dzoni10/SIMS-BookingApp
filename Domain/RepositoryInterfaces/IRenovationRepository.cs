using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IRenovationRepository
    {
        public void Save(Renovation renovation);
        public Renovation GetById(int id);
        public void Update(Renovation renovation);
        public void Delete(Renovation renovation);
        public List<Renovation> GetAll();
    }
}
