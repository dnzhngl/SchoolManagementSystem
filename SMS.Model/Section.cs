using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Section : Entity<int>
    {
        public Section()
        {
            Students = new HashSet<Student>();
            Timetables = new HashSet<Timetable>();
        }
        public string SectionName { get; set; }

        public int StudentCapacity { get; set; }

        public int NumberOfStudentsEnrolled { get; set; }

        [ForeignKey("Instructor")]
        public Nullable<int> AdvisoryTeacherId { get; set; }
        public virtual Instructor AdvisoryTeacher { get; set; }

        [ForeignKey("Grade")]
        public Nullable<int> GradeId { get; set; }
        public virtual Grade Grade { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
