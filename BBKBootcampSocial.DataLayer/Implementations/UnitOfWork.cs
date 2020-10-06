using System.Threading.Tasks;
using BBKBootcampSocial.Core.IServices;

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
