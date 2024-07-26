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
    public class ForumCommentRepository : IForumCommentRepository
    {
        Repository<ForumComment> forumCommentRepository;
        public Subject forumCommentSubject;

        public ForumCommentRepository()
        {
            forumCommentRepository = new Repository<ForumComment>();
            forumCommentSubject = new Subject();
        }
        public List<ForumComment> GetAll() {
            return forumCommentRepository.GetAll();
        }
        public void Save(ForumComment forumComment)
        {
            forumCommentRepository.Save(forumComment);
            forumCommentSubject.NotifyObservers();
        }
        public void Delete(ForumComment forumComment)
        {
            forumCommentRepository.Delete(forumComment);
            forumCommentSubject.NotifyObservers();
        }
        public ForumComment GetById(int id)
        {
            return forumCommentRepository.FindId(id);
        }
        public void Update(ForumComment forumComment)
        {
            forumCommentRepository.Update(forumComment);
            forumCommentSubject.NotifyObservers();
        }

    }
}
