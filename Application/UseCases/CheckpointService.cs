using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.DTO;
using BookingApp.Model;
using iText.StyledXmlParser.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace BookingApp.Application.UseCases
{
    public class CheckpointService
    {
        private ICheckpointRepository checkpointRepository;
        public CheckpointService(ICheckpointRepository checkpointRepository) 
        {
            this.checkpointRepository = checkpointRepository;
        }

        public Checkpoint Save(Checkpoint checkpoint)
        {
            return checkpointRepository.Save(checkpoint);
        }

        public Checkpoint GetById(int id)
        {
            return checkpointRepository.GetById(id);
        }

        public List<Checkpoint> GetAll()
        {
            return checkpointRepository.GetAll();
        }

        public List<Checkpoint> GetAllByTourId(int id)
        {
            return checkpointRepository.GetAll().Where(checkpoint => checkpoint.TourId == id).ToList();
        }

        public List<Checkpoint> LoadCheckpoints(int id)
        {
            List<Checkpoint> result = new List<Checkpoint>();
            foreach (Checkpoint checkpoint in checkpointRepository.GetAll())
            {
                if (id == checkpoint.TourId)
                {
                    result.Add(checkpoint);
                }
            }
            return result;
        }
    }
}
