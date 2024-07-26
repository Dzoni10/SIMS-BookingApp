using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ICommentRepository
    {
        public List<Comment> GetAll();
        public Comment Save(Comment comment);
        public void Delete(Comment comment);
        public Comment Update(Comment comment);
        public Comment GetById(int id);
        public List<Comment> FindCommentByUser(User user);

    }
}
