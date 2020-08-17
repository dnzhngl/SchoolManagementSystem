using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class ExamType : Entity<int>
    {
        public string ExamTypeName { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }
}
