using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dialogistic.Abstract
{
    public interface IDialogisticRepository<TEntity> where TEntity : class
    {
        /* Methods for getting objects */
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /* Methods for adding objects */
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        /* Methods for removing objects */
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        /* Utility methods */
        int Count();
    }
}
