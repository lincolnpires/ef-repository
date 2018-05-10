using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Source
{
    public interface IEFRepository<T> where T : Entity
    {
        T GetOneById(Guid id);

        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task RemoveAsync(T entity);
    }
}
