using System.Threading;
using System.Threading.Tasks;
using BBKBootcampSocial.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BBKBootcampSocial.DataLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructor

        private BBKDatabaseContext db;

        public UnitOfWork(BBKDatabaseContext db)
        {
            this.db = db;
        }

        #endregion

        #region Implementations

        public async Task<int> SaveChanges()
        {
            return await db.SaveChangesAsync();
        }

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await db.Database.BeginTransactionAsync();
        }

        public IDbContextTransaction? GetDbContextTransaction(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return db.Database.CurrentTransaction;
        }

        public void RollBackChanges(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var transaction = GetDbContextTransaction(cancellationToken);

            if (transaction != null)
            {
                db.Database.RollbackTransaction();
                transaction.Dispose();
            }
        }

        public void CommitTrasaction(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            db.Database.CommitTransaction();
        }

        async Task<TRepository> IUnitOfWork.GetRepository<TRepository, TEntity>()
        {
            await Task.CompletedTask;
            return new GenericRepository<TEntity>(db) as TRepository;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            db?.Dispose();
        }

        #endregion
    }
}
