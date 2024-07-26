using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;
using System.Runtime.CompilerServices;
using BookingApp.Domain.Models;

namespace BookingApp.DTO
{
    public class ImageDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (path != value)
                {
                    path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        private int entityId;
        public int EntityId
        {
            get { return entityId; }
            set
            {
                if (entityId != value)
                {
                    entityId = value;
                    OnPropertyChanged("EntityId");
                }
            }
        }

        private EntityType type;

        public EntityType Type
        {
            get
            {
                if (type == EntityType.a)
                {
                    return EntityType.a;
                }
                else
                {
                    return EntityType.t;
                }
            }
            set
            {
                if(value == EntityType.a && type != EntityType.a)
                {
                    type = EntityType.a;
                    OnPropertyChanged("Type");
                }
                else if(value == EntityType.t && type != EntityType.t)
                {
                    type = EntityType.t;
                    OnPropertyChanged("Type");
                }

            }
        }

        

        public ImageDTO()
        {

        }

        public Image ToImage()
        {
            return new Image(Id,path,entityId,type);
        }

        public ImageDTO(string path,int entityId,EntityType entityType)

        {
            this.path = path;
            this.entityId = entityId;
            this.type = entityType;

        }

        public ImageDTO(int id,string path, int entityId, EntityType entityType)
        {
            Id = id;
            this.path = path;
            this.entityId = entityId;
            this.type = entityType;
        }

        public ImageDTO(Image image)
        {
            Id = image.Id;
            path = image.Path;
            entityId = image.EntityId;
            type = image.EntityType;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
