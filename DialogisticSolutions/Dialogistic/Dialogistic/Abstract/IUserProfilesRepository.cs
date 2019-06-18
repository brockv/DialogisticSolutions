using Dialogistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogistic.Abstract
{
    public interface IUserProfilesRepository : IDialogisticRepository<UserProfile>
    {
        IEnumerable<UserProfile> GetUserProfiles();
    }
}
