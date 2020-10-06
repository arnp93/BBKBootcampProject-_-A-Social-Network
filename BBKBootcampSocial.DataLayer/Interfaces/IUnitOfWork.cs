using System;
using System.Threading.Tasks;
using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.DataLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<TRepository> GetRepository<TRepository, TEntity>() where TRepository : class, IGenericRepository<TEntity> where TEntity : BaseEntity;

        Task<int> SaveChanges();
    }
}
