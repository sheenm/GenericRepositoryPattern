using System;
using System.Collections.Generic;


namespace GenericRepositoryPattern.Repositories
{
    public interface IRepository<T>
    {
        T GetById(int id);

        IEnumerable<T> GetByQuery(Func<T, bool> selector);

        bool Save(T item);
    }
}