using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class Parent : Entity<int>
    {
        public Parent()
        {
            Students = new HashSet<Student>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Address { get; set; }

        public ICollection<Student> Students  { get; set; }
    }
}
