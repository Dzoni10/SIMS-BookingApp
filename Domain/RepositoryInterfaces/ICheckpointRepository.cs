using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ICheckpointRepository
    {
        public Checkpoint Save(Checkpoint checkpoint);
        public void SaveAll(List<Checkpoint> checkpoints);
        public void Update(Checkpoint checkpoint);
        public Checkpoint GetById(int id);
        public List<Checkpoint> GetAll();
    }
}
