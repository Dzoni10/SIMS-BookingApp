using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ForumRepository : IForumRepository
    {
        Repository<Forum> forumRepository;
        public Subject forumSubject;

        public ForumRepository()
        {
            forumRepository = new Repository<Forum>();
            forumSubject = new Subject();
        }
        public List<Forum> GetAll()
        {
            return forumRepository.GetAll();
        }

        public void Save(Forum forum)
        {
            forumRepository.Save(forum);
            forumSubject.NotifyObservers();
        }

        public  void Delete(Forum forum)
        {
            forumRepository.Delete(forum);
            forumSubject.NotifyObservers();
        }

        public Forum GetById(int id)
        {
            return forumRepository.FindId(id);
        }

        public void Update(Forum forum)
        {
            forumRepository.Update(forum);
            forumSubject.NotifyObservers();
        }
    }
}
