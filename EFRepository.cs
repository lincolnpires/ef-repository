using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Source
{
    public class EFRepository<T> : IDisposable, IEFRepository<T> where T : Entity
    {
        protected readonly DbContext dbContext;
        private bool disposed;

        public EFRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.disposed = false;
        }

        public T GetOneById(Guid id)
        {
            return dbContext.Set<T>()
                .DefaultIfEmpty(null)
                .SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .AsQueryable();
        }

        public IQueryable<T> GetAll()
        {
            return dbContext.Set<T>()
                .AsNoTracking()
                .AsQueryable();
        }

        public async Task AddAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                dbContext.Dispose();
                this.disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}
