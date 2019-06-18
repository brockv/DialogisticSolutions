using Dialogistic.Abstract;
using Dialogistic.DAL;
using Dialogistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogistic.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DialogisticContext _context;

        public UnitOfWork(DialogisticContext context)
        {
            _context = context;
            Constituents = new ConstituentsRepository(_context);
            Changes = new ChangesRepository(_context);
            Assignments = new AssignmentsRepository(_context);
            Logs = new CallLogsRepository(_context);
            Profiles = new UserProfilesRepository(_context);
            Gifts = new GiftsRepository(_context);
        }

        public IConstituentsRepository Constituents { get; private set; }
        public IChangesRepository Changes { get; private set; }
        public IAssignmentsRepository Assignments { get; private set; }
        public ICallLogsRepository Logs { get; private set; }
        public IUserProfilesRepository Profiles { get; private set; }
        public IGiftsRepository Gifts { get; private set; }

        public System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return _context.Entry(entity);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
