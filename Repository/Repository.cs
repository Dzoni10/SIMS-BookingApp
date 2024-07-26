using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using BookingApp.Domain.Models;

namespace BookingApp.Repository
{
    //Generic class for repositories
    public class Repository<T> where T : class, ISerializable, new()
    {
        private string FilePath = "../../../Resources/Data/";
        private List<T> _data = new List<T>();
        private Serializer<T> _serializer; 

        public Repository()
        {
            FilePath += typeof(T).Name + ".csv";
            _serializer = new Serializer<T>();
            _data = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _data = _serializer.FromCSV(FilePath);
            if(_data.Count < 1 ) 
            {
                return 1;
            }
            return _data.Max(m => m.Id) + 1;
        }
        public T Save(T repo)
        {
            repo.Id = NextId();
            _data = _serializer.FromCSV(FilePath);
            _data.Add(repo);
            _serializer.ToCSV(FilePath, _data);
            return repo;
        }
        public void SaveAll(List<T> obj)
        {
            foreach(T item in obj)
            {
                item.Id = NextId();
                _data = _serializer.FromCSV(FilePath);
                _data.Add(item);
                _serializer.ToCSV(FilePath, _data);
            }
        }

        public List<T> GetAll()
        {
            return _data;
        }

        public void Update(T obj)
        {
            _data = _serializer.FromCSV(FilePath);
            T current  = _data.Find(dat=>dat.Id == obj.Id);
            int idx = _data.IndexOf(current);
            _data.Remove(current);
            _data.Insert(idx, obj);
            _serializer.ToCSV(FilePath, _data);
        }

        public void Delete(T obj)
        {
            _data = _serializer.FromCSV(FilePath);
            T founded = _data.Find(dat => dat.Id == obj.Id);  
            _data.Remove(founded);
            _serializer.ToCSV(FilePath, _data);
        }

        public T FindId(int id)
        {
            return _data.Find(dat => dat.Id == id);
        }

        //Special method for CommentRepository which return comment from specify User
        
        public List<T> FindCommentByUser(User obj)
        {
            return _data.FindAll(dat=>dat.Id == obj.Id);
        }

        public T? FindByName(Func<T, string> nameSelector, string name)
        {
            return _data.Find(dat => nameSelector(dat) == name);
        }
    }
}
