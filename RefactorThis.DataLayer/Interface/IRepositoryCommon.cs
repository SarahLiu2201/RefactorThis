using System;
using System.Threading.Tasks;

namespace RefactorThis.DataLayer
{
    public interface IRepositoryCommon<TEntity, TKey> : IDisposable where TEntity : class, IEntity<TKey>
    {
        TEntity GetById(TKey id);
        Task<TEntity> GetAsync(TKey id);            
    }
}
