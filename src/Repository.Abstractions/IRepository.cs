using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Abstractions
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetByQueryAsync(Func<T, bool> selector);

        Task<bool> SaveAsync(T item);

        Task<int> CreateAsync(T item);
    }
}