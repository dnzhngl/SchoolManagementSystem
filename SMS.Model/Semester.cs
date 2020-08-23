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
        }
        public DateTime SemesterBeginning { get; set; }
        public DateTime SemesterEnd { get; set; }

        public ICollection<Timetable> Timetables { get; set; }

    }
}
