using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Repository.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);
        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        Task DaleteAsync(object id);
        Task<object> SaveAsync();
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> query);
    }
}
