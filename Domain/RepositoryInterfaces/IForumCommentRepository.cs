using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IForumCommentRepository
    {
        public List<ForumComment> GetAll();
        public ForumComment GetById(int id);
        public void Save(ForumComment comment);
        public void Delete(ForumComment forumComment);
        public void Update(ForumComment comment);
    }
}
