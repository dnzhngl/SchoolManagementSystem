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
            Admins = new HashSet<Admin>();
            Instructors = new HashSet<Instructor>();
            Parents = new HashSet<Parent>();
            Students = new HashSet<Student>();
        }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Parent> Parents { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
