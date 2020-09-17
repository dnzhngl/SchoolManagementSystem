using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class ExamType : Entity<int>
    {
        public ExamType()
        {
            Exams = new HashSet<Exam>();
        }
        public string ExamTypeName { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
