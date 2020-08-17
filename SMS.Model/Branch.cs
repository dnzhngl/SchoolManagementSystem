using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SMS.Model
{
    public class Branch : Entity<int>
    {
        public Branch()
        {
            Instructors = new HashSet<Instructor>();
        }

        public string BranchName { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
    }
}
