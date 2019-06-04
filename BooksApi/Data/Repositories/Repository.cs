using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BooksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            DbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int skip, int take, Expression<Func<T, bool>> filter = default)
        {
            var query = DbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var list = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return list;
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task AddAsync(T item)
        {
            await DbSet.AddAsync(item);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var item = await GetAsync(id);
            if (item == null)
            {
                return false;
            }

            DbSet.Remove(item);
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await DbSet.AnyAsync(filter);
        }
    }
}