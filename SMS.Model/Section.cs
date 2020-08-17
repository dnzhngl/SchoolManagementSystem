using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Section : Entity<int>
    {
        public Section()
        {
            Students = new HashSet<Student>();
            TimeTables = new HashSet<TimeTable>();
        }

        public string SectionName { get; set; }

        public int StudentCapacity { get; set; }


        [ForeignKey("Grade")]
        public Nullable<int> GradeId { get; set; }
        public Grade Grade { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<TimeTable> TimeTables { get; set; }
    }
}
