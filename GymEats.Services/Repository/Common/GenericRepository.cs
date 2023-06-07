using GymEats.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Repository.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly GymEatsDbContext _context;
        private DbSet<T> table = null;
        public GenericRepository(GymEatsDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await table.FindAsync(id);
        }
        public async Task InsertAsync(T obj)
        {
            await table.AddAsync(obj);
        }
        public async Task UpdateAsync(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public async Task DaleteAsync(object id)
        {
            var entity = await table.FindAsync(id);
            if (entity != null)
            {
                table.Remove(entity);
            }
        }

        public async Task<object> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> query)
        {
            return table.Where(query);
        }
    }
}
