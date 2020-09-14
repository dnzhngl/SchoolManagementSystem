using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Admin : Entity<int>
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string Duty { get; set; }

        //[ForeignKey("Role")]
        //public Nullable<int> RoleId { get; set; }
        //public virtual Role Role { get; set; }

        [ForeignKey("User")]
        public Nullable<int> UserId { get; set; }
        public virtual User User { get; set; }
    }
}
