using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class TimetableView : Entity<int>
    {
        public string ClassroomName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public string Instructor { get; set; }
        public string DayName { get; set; }
        public string LessonBeginTime { get; set; }
        public string LessonEndTime { get; set; }
    }
}
