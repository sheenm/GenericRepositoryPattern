using System;
using System.Collections.Generic;
using System.Data;

namespace GenericRepositoryPattern.Repositories
{
    public abstract class AdoRepository<T> : IRepository<T>
    {
        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetByQuery(Func<T, bool> selector)
        {
            throw new NotImplementedException();
        }

        public void Save(T item)
        {
            throw new NotImplementedException();
        }

        protected abstract T PopulateRecord(IDataReader reader);
    }
}