using System.Collections.Generic;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class ImageRepository : IImageRepository
    {
        Repository<Image> imageRepository;

        public ImageRepository()
        {
            imageRepository = new Repository<Image>();
        }

        public void Save(Image image)
        {
            imageRepository.Save(image);
        }

        public void SaveAll(List<Image> images)
        {
            imageRepository.SaveAll(images);
        }

        public void Update(Image image)
        {
            imageRepository.Update(image);
        }

        public Image GetById(int id)
        {
            return imageRepository.FindId(id);
        }

        public List<Image> GetAll()
        {
            return imageRepository.GetAll();
        }

        
    }
}
