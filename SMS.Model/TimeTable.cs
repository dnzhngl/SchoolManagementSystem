using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class TimeTable : Entity<int>
    {
        public string ClassroomName { get; set; }


        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public Section Section { get; set; }


        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }


        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }


        [ForeignKey("Day")]
        public int DayId { get; set; }
        public Day Day { get; set; }


        [ForeignKey("LessonTime")]
        public int LessonTimeId { get; set; }
        public LessonTime LessonTime { get; set; }

    }
}
