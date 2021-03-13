using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Repositories.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();

        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Attach(TEntity entity);
        void Update(TEntity entity);
        void Modify(TEntity entity);
        void Delete(TEntity entity);
    }
}
