using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class TimetableDTO
    {

        public int Id { get; set; }

        public int SectionId { get; set; }
        public SectionDTO SectionDTO { get; set; }

        public int SubjectId { get; set; }
        public SubjectDTO SubjectDTO { get; set; }

        public int InstructorId { get; set; }
        public InstructorDTO InstructorDTO { get; set; }

        public int DayId { get; set; }
        public DayDTO DayDTO { get; set; }

        public int LessonTimeId { get; set; }
        public LessonTimeDTO LessonTimeDTO { get; set; }

        public int SemesterId { get; set; }
        public SemesterDTO SemesterDTO { get; set; }

        public int ClassroomId { get; set; }
        public ClassroomDTO ClassroomDTO { get; set; }



    }
}
