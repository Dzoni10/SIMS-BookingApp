using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IForumRepository
    {
        public List<Forum> GetAll();
        public void Save(Forum forum);
        public void Delete(Forum forum);
        public Forum GetById(int id);
        public void Update(Forum forum);
    }
}
