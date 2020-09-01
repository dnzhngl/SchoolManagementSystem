using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class TimetableViewDTO
    {
        public int Id { get; set; }
        public string ClassroomName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public string Instructor { get; set; }
        public string DayName { get; set; }
        public string LessonPeriod { get; set; }

    }
}
