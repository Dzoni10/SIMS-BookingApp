﻿using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IIgnoreRepository
    {
        public void Save(Ignore ignore);
        public List<Ignore> GetAll();
    }
}
