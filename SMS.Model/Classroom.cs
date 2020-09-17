using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class Classroom : Entity<int>
    {
        public Classroom()
        {
            Timetables = new HashSet<Timetable>();
        }
        public string ClassroomName { get; set; }
        public int StudentCapacity { get; set; }
        public virtual ICollection<Timetable> Timetables { get; set; }

    }
}
