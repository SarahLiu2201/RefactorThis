using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.DataLayer
{
    public abstract class RepositoryCommon<TDbContext, TEntity, TKey> : IRepositoryCommon<TEntity, TKey>, IDisposable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>
    {
        protected TDbContext dbContext { get; }
        protected bool Disposed;

        public RepositoryCommon(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
        }

        public Task<TEntity> GetAsync(TKey id)
        {
            return dbContext.Set<TEntity>().Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public TEntity GetById(TKey id)
        {
            return dbContext.Set<TEntity>().Where(x => x.Id.Equals(id)).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void EntityChanged(TEntity entity)
        {
            var entry = dbContext.Entry(entity);

            switch (entry.State)
            {
                case EntityState.Detached:
                    dbContext.Set<TEntity>().Add(entity);
                    break;
                case EntityState.Modified:
                    dbContext.Set<TEntity>().Update(entity);
                    break;
                case EntityState.Added:
                    dbContext.Set<TEntity>().Add(entity);
                    break;
                case EntityState.Unchanged:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
