using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class TimeTableDTO
    {
        public int Id { get; set; }
        public string ClassroomName { get; set; }


        public int SectionId { get; set; }
        public SectionDTO SectionDTO { get; set; }

        public int SubjectId { get; set; }
        public SubjectDTO Subject { get; set; }

        public int InstructorId { get; set; }
        public InstructorDTO Instructor { get; set; }

        public int DayId { get; set; }
        public DayDTO Day { get; set; }

        public int LessonTimeId { get; set; }
        public LessonTimeDTO LessonTimeDTO { get; set; }

        public int SemesterId { get; set; }
        public SemesterDTO SemesterDTO { get; set; }

    }
}
