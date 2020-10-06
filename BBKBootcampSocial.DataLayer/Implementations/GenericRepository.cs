using BBKBootcampSocial.Domains.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using BBKBootcampSocial.DataLayer.Interfaces;

namespace BBKBootcampSocial.DataLayer.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {

        #region Constructor

        private BBKDatabaseContext context;
        private DbSet<TEntity> dbset;


        public GenericRepository(BBKDatabaseContext context)
        {
            this.context = context;

            this.dbset = this.context.Set<TEntity>();
        }

        #endregion

        #region Properties

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.IsDelete = false;
            await dbset.AddAsync(entity);
        }

        public System.Linq.IQueryable<TEntity> GetEntitiesQuery()
        {
            return dbset.AsQueryable();
        }

        public async Task<TEntity> GetEntityById(long EntityId)
        {
            return await dbset.SingleOrDefaultAsync(e => e.Id == EntityId);
        }

        public void RemoveEntity(TEntity entity)
        {
            entity.IsDelete = true;
            UpdateEntity(entity);
        }

        public async Task RemoveEntity(long id)
        {
            var entity = await GetEntityById(id);
            RemoveEntity(entity);
        }

        public void UpdateEntity(TEntity entity)
        {
            dbset.Update(entity);
        }


        #endregion

        #region Dispose

        public void Dispose()
        {
            context?.Dispose();
        }

        #endregion
    }
}
