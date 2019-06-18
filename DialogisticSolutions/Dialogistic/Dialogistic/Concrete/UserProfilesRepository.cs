using Dialogistic.Abstract;
using Dialogistic.DAL;
using Dialogistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialogistic.Concrete
{
    public class UserProfilesRepository : DialogisticRepository<UserProfile>, IUserProfilesRepository
    {
        public UserProfilesRepository(DialogisticContext context) : base(context)
        {
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            return DialogisticContext.UserProfiles.ToList();
        }

        public DialogisticContext DialogisticContext
        {
            get { return Context as DialogisticContext; }
        }
    }
}