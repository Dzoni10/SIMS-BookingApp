using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using BookingApp.WPF.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BookingApp.Repository
{
    public class CommentRepository : ICommentRepository
    {

        Repository<Comment> commentRepository;

        public CommentRepository()
        {
            commentRepository = new Repository<Comment>();    
        }

        public List<Comment> GetAll()
        {
            return commentRepository.GetAll();
        }

        //Modified Update method for CommentForm.xaml.cs
        public Comment Save(Comment comment)
        {
            commentRepository.Save(comment);
            return comment;
        }

        public void Delete(Comment comment)
        {
            commentRepository.Delete(comment);
        }
        //Modified Update method for CommentForm.xaml.cs
        public Comment Update(Comment comment)
        {
            commentRepository.Update(comment);
            return comment;
        }

        public Comment GetById(int id)
        {
            //_comments = _serializer.FromCSV(FilePath);
            //return _comments.FindAll(c => c.User.Id == user.Id);
            return commentRepository.FindId(id);
        }

        public List<Comment> FindCommentByUser(User user)
        {
            return commentRepository.FindCommentByUser(user);
        }
    }
}
