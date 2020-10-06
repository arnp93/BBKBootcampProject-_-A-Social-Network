using System;
using System.Threading.Tasks;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.Common;

namespace BBKBootcampSocial.Core.IServices
{
    public interface IUnitOfWork : IDisposable
    {
        Task<TRepository> GetRepository<TRepository, TEntity>() where TRepository : class, IGenericRepository<TEntity> where TEntity : BaseEntity;

        Task<int> SaveChanges();
    }
}
