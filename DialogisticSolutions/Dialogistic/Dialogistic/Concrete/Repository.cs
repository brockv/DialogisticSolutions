//using Dialogistic.Abstract;
//using System.Data.Linq;
//using System.Linq;

//namespace Dialogistic.Concrete
//{
//    public class Repository<T> : IRepository<T> where T : class
//    {
//        public DataContext Context
//        {
//            get;
//            set;
//        }

//        public virtual IQueryable<T> GetAll()
//        {
//            return Context.GetTable<T>();
//        }

//        public virtual void InsertOnSubmit(T entity)
//        {
//            GetTable().InsertOnSubmit(entity);
//        }

//        public virtual void DeleteOnSubmit(T entity)
//        {
//            GetTable().DeleteOnSubmit(entity);
//        }

//        public virtual void SubmitChanges()
//        {
//            Context.SubmitChanges();
//        }

//        public virtual ITable GetTable()
//        {
//            return Context.GetTable<T>();
//        }
//    }
//}