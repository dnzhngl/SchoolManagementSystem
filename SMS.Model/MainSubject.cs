using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class MainSubject : Entity<int>
    {
        public MainSubject()
        {
            Subjects = new HashSet<Subject>();
        }

        public string MainSubjectName { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
