using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class Role : Entity<int>
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}
