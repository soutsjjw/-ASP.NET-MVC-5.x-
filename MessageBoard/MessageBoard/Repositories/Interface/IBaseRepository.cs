﻿using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MessageBoard.Repositories.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        /// <summary>
        /// Gets entities.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="orderBy">The order by to apply.</param>
        /// <param name="includeProperties">includerd properties.</param>
        /// <returns>The query result.</returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Get object from table by id.
        /// </summary>
        /// <param name="id">The object id.</param>
        /// <returns>The query result.</returns>
        TEntity GetByID(object id);

        /// <summary>
        /// Inset entity in table.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The created item Id.</returns>
        string Insert(TEntity entity);

        /// <summary>
        /// Inset entities in table.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        void InsertRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity by id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        void Delete(object id);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entityToUpdate">The entity updated.</param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Update all entities.
        /// </summary>
        /// <param name="entitiesToUpdate">The entities list to update.</param>
        void UpdateAll(IEnumerable<TEntity> entitiesToUpdate);
    }
}
