using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Instructor : Entity<int>
    {
        public Instructor()
        {
            TimeTables = new HashSet<TimeTable>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string Duty { get; set; }

        [ForeignKey("Branch")]
        public Nullable<int> BranchId { get; set; }
        public Branch Branch { get; set; }

        public ICollection<TimeTable> TimeTables { get; set; }
    }
}
