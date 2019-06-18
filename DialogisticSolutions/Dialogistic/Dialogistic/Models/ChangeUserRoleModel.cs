using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dialogistic.Models
{
    public class ChangeUserRoleModel
    {
        public virtual UserProfile User { get; set; }

        public string Role { get; set; }        
    }
}