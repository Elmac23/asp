using System;
using System.Collections.Generic;
using Lista4.Models;

namespace Lista4.Services
{
    public interface IDapperRepository : IDisposable
    {
        IEnumerable<Person> GetAll();
        Person? GetById(int id);
        void Add(Person person);
    }
}
