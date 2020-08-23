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
            Timetables = new HashSet<Timetable>();
        }
        public string LessonBeginTime { get; set; }
        public string LessonEndTime { get; set; }
        public ICollection<Timetable> Timetables { get; set; }
    }
}
