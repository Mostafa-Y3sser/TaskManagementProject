using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Task_Management.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? Filter = null, string? IncludeProperties = null);

        Task<T> GetAsync(Expression<Func<T, bool>> Filter, string? IncludeProperties = null);

        Task AddAsync(T Entity);

        void Update(T Entity);

        Task RemoveAsync(int ID);

        Task RemoveRangeAsync(IEnumerable<T> Entities);
    }
}
