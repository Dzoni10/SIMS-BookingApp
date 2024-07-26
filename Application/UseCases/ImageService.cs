using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;

namespace BookingApp.Application.UseCases
{
    public class ImageService
    {
        private IImageRepository imageRepository;
        private static string projectAbsolutePath;

        public ImageService(IImageRepository imageRepository) 
        {
            this.imageRepository = imageRepository;

            //string projectPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            //projectAbsolutePath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(projectPath)));
        }
        public ImageService()
        {
            this.imageRepository = Injector.Injector.CreateInstance<IImageRepository>();
        }
        public void Save(Image image)
        {
            imageRepository.Save(image);
        }

        public Image GetById(int id)
        {
            return imageRepository.GetById(id);
        }
        public Image GetImageForAccommodation(int id)
        {
            List<Image> images = imageRepository.GetAll();
            foreach (Image image in images)
            {
                if (image.EntityType == EntityType.a && image.EntityId == id)
                {
                    image.Path = "../../Resources/Images/" + image.Path;
                    return image;
                }
            }
            return null;
        }

        public List<Image> GetImagesForRateAccommodation(int reservationId)
        {
            List<Image> images = new List<Image>();
            foreach (Image image in imageRepository.GetAll())
            {
                if (image.EntityType == EntityType.ra && image.EntityId == reservationId)
                {
                    images.Add(image);
                }
            }
            return images;
        }

        public Image GetImageForTours(int id)
        {
            List<Image> images = imageRepository.GetAll();
            foreach (Image image in images)
            {
                if (image.EntityType == EntityType.t && image.EntityId == id)
                {
                    return image;
                }
            }
            return null;
        }

        public List<Image> GetImagesForTour(int id)
        {
            List<Image> images = new List<Image>();
            foreach (Image image in imageRepository.GetAll())
            {
                if (image.EntityType == EntityType.t && image.EntityId == id)
                {
                    images.Add(image);
                }
            }
            return images;
        }

        public List<Image> GetAll()
        {
            return imageRepository.GetAll();
        }
        /*
        public static string GetAbsolutePath(string relativePath)
        {
            string projectPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            projectAbsolutePath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(projectPath)));

            if (relativePath == null)
            {
                return null;
            }
            string destinationFilePath = System.IO.Path.Combine(projectAbsolutePath, relativePath);
            return destinationFilePath;
        }

        //saves images in a folder within the project and returns a list of relative paths.
        public List<string> SaveImages(string[] images, string relativPath)
        {
            List<string> imageRelativePaths = new List<string>();
            foreach (string image in images)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image);

                string destinationFilePath = System.IO.Path.Combine(GetAbsolutePath(relativPath), uniqueFileName);

                System.IO.File.Copy(image, destinationFilePath);

                imageRelativePaths.Add(System.IO.Path.Combine(relativPath, uniqueFileName));
            }
            return imageRelativePaths;
        }

        public static void DeleteImages(List<string> absoluteImagesPaths)
        {
            foreach (string absoluteImagePath in absoluteImagesPaths)
            {
                if (System.IO.File.Exists(absoluteImagePath))
                {
                    System.IO.File.Delete(absoluteImagePath);
                }
            }
        }
        */
    }
}
