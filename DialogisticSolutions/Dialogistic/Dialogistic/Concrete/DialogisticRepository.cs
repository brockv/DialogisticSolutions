using Dialogistic.Abstract;
using Dialogistic.DAL;
using Dialogistic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Dialogistic.Concrete
{
    /// <summary>
    /// Generic repository used by all other repositories, and subsequently, their interfaces
    /// </summary>
    /// <typeparam name="T">Represents a table/model. Used to construct the repository for a specific instance.</typeparam>
    public class DialogisticRepository<TEntity> : IDialogisticRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public DialogisticRepository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Generic Get method implemented by all interfaces.
        /// </summary>
        /// <param name="id">The id of the object to get.</param>
        /// <returns></returns>
        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public int Count()
        {
            return Context.Set<TEntity>().Count();
        }
    }
}