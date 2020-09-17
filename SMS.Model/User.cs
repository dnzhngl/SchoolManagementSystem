using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class User : Entity<int>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? LoginTimeStamps { get; set; }
        public DateTime? FailedLogin { get; set; }


        [ForeignKey("Role")]
        public Nullable<int> RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }
}
