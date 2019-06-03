using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BooksApi.Data.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(int skip, int take, Expression<Func<T, bool>> filter = default);

        Task<T> GetAsync(Guid id);

        Task AddAsync(T item);

        Task<bool> RemoveAsync(Guid id);

        Task<int> CountAsync();
    }
}