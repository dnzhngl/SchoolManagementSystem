using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class LessonTime : Entity<int>
    {
        public LessonTime()
        {
            TimeTables = new HashSet<TimeTable>();
        }
        public string LessonTimePeriod { get; set; }
        public ICollection<TimeTable> TimeTables { get; set; }
    }
}
