using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Timetable : Entity<int>
    {

        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }


        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }


        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }


        [ForeignKey("Day")]
        public int DayId { get; set; }
        public virtual Day Day { get; set; }


        [ForeignKey("LessonTime")]
        public int LessonTimeId { get; set; }
        public virtual LessonTime LessonTime { get; set; }


        [ForeignKey("Semester")]
        public int SemesterId { get; set; }
        public virtual Semester Semester { get; set; }


        [ForeignKey("Classroom")]
        public int ClassroomId { get; set; }
        public virtual Classroom Classroom { get; set; }

    }
}
