using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {
        Repository<ComplexTourRequest> complexTourRequestrepository;

        public ComplexTourRequestRepository()
        {
            complexTourRequestrepository = new Repository<ComplexTourRequest>();
        }

        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest)
        {
            return complexTourRequestrepository.Save(complexTourRequest);
        }

        public void SaveAll(List<ComplexTourRequest> complexTourRequests)
        {
            complexTourRequestrepository.SaveAll(complexTourRequests);
        }

        public void Update(ComplexTourRequest complexTourRequest)
        {
            complexTourRequestrepository.Update(complexTourRequest);
        }

        public ComplexTourRequest GetById(int id)
        {
            return complexTourRequestrepository.FindId(id);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return complexTourRequestrepository.GetAll();
        }
    }
}
