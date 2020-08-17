using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Subject : Entity<int>
    {
        public Subject()
        {
            Exams = new HashSet<Exam>();
            TimeTables = new HashSet<TimeTable>();
        }

        public string SubjectName { get; set; }
        public int WeeklyCourseHours { get; set; }

        [ForeignKey("Branch")]
        public Nullable<int> MainSubjectId { get; set; }
        public MainSubject MainSubject { get; set; }

        public ICollection<Exam> Exams { get; set; }

        public ICollection<TimeTable> TimeTables { get; set; }
    }
}
