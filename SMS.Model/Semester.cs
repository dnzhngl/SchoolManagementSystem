using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class Semester : Entity<int>
    {
        public Semester()
        {
            Timetables = new HashSet<Timetable>();
            Certificates = new HashSet<Certificate>();
        }
        public string SemesterName { get; set; }
        public DateTime SemesterBeginning { get; set; }
        public DateTime SemesterEnd { get; set; }
        public string AcademicYear { get; set; }

        public virtual ICollection<Timetable> Timetables { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}
