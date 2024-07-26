using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IImageRepository
    {
        public void Save(Image image);
        public void SaveAll(List<Image> images);
        public void Update(Image image);
        public Image GetById(int id);
        public List<Image> GetAll();
    }
}
