using System;
using System.Collections.Generic;
using Lista4.Models;

namespace Lista4.Services
{
    public class DapperRepository : IDapperRepository
    {
        private readonly List<Person> _data = new();
        private bool _disposed;

        public DapperRepository()
        {
            // seed
            _data.Add(new Person { Id = 1, Name = "Alice", Age = 30 });
            _data.Add(new Person { Id = 2, Name = "Bob", Age = 25 });
        }

        public IEnumerable<Person> GetAll()
        {
            ThrowIfDisposed();
            return _data;
        }

        public Person? GetById(int id)
        {
            ThrowIfDisposed();
            return _data.Find(p => p.Id == id);
        }

        public void Add(Person person)
        {
            ThrowIfDisposed();
            person.Id = _data.Count + 1;
            _data.Add(person);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(DapperRepository));
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}
