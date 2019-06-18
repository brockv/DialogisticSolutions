using Dialogistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogistic.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IConstituentsRepository Constituents { get; } // Constituents table/model
        IChangesRepository Changes { get; }           // ProposedConstituentChanges table/model
        IAssignmentsRepository Assignments { get; }   // CallAssignments table/model
        ICallLogsRepository Logs { get; }          // CallLogs table/model
        IUserProfilesRepository Profiles { get; }     // UserProfiles table/model
        IGiftsRepository Gifts { get; }               // Gifts table/model

        int Complete();
        System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
