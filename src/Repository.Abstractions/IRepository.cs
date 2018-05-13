using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Abstractions
{
    public interface IRepository<T>
    {
        ///<summary>
        /// Gets a single object form the repo
        ///</summary>
        Task<T> GetByIdAsync(int id);

        ///<summary>
        /// Gets IEnumerable of entities, selected by a query
        ///</summary>
        Task<IEnumerable<T>> GetByQueryAsync(Func<T, bool> selector);

        ///<summary>
        /// Saves an entity in the repo
        ///</summary>
        ///<returns>was result successful or not</returns>
        Task<bool> SaveAsync(T item);

        ///<summary>
        /// Creates an new Entity in the repo
        ///</summary>
        ///<returns>Id of new item</returns>
        Task<int> CreateAsync(T item);
    }
}