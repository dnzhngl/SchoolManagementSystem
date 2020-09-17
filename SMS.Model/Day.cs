using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class Day : Entity<int>
    {
        public Day()
        {
            Timetables = new HashSet<Timetable>();
        }
        public string DayName { get; set; }
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
