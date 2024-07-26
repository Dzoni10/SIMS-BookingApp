using System.Collections.Generic;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;

namespace BookingApp.Repository
{
    public class CheckpointRepository : ICheckpointRepository
    {
        Repository<Checkpoint> checkpointRepository;

        public CheckpointRepository()
        {
            checkpointRepository = new Repository<Checkpoint>();
        }

        public Checkpoint Save(Checkpoint checkpoint)
        {
            return checkpointRepository.Save(checkpoint);
        }

        public void SaveAll(List<Checkpoint> checkpoints)
        {
            checkpointRepository.SaveAll(checkpoints);
        }

        public void Update(Checkpoint checkpoint)
        {
            checkpointRepository.Update(checkpoint);
        }

        public Checkpoint GetById(int id)
        {
            return checkpointRepository.FindId(id);
        }

        public List<Checkpoint> GetAll()
        {
            return checkpointRepository.GetAll();
        }
    }
}
