using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Abstractions
{
    public interface IRepository<T>
    {
        ///<summary>
        /// Gets a single item form the repo
        ///</summary>
        Task<T> GetByIdAsync(int id);

        ///<summary>
        /// Gets All items of an entity
        ///</summary>
        Task<IEnumerable<T>> GetAllAsync();

        ///<summary>
        /// Saves an item in the repo
        ///</summary>
        ///<returns>was result successful or not</returns>
        Task<bool> SaveAsync(T item);

        ///<summary>
        /// Creates an new item of an entity in the repo
        ///</summary>
        ///<returns>Id of new item</returns>
        Task<int> CreateAsync(T item);
    }
}