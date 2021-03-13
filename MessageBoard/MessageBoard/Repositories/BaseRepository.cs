using MessageBoard.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
         where TEntity : class
    {
        protected DbContext DbContext { get; }

        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual IQueryable<TEntity> Query()
        {
            return DbContext.Set<TEntity>();
        }

        public void Insert(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
        }

        public void Attach(TEntity entity)
        {
            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }
        public void Modify(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void Refresh(TEntity entity)
        {
            DbContext.Entry<TEntity>(entity).Reload();
        }
    }
}
