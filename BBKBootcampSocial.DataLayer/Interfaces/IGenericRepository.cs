using BBKBootcampSocial.Domains.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BBKBootcampSocial.DataLayer.Interfaces
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetEntitiesQuery();

        Task<TEntity> GetEntityById(long EntityId);

        Task AddEntity(TEntity entity);

        void UpdateEntity(TEntity entity);

        void RemoveEntity(TEntity entity);

        Task RemoveEntity(long id);
        void DeleteEntity(TEntity entity);
        Task DeleteEntity(long id);
    }
}
