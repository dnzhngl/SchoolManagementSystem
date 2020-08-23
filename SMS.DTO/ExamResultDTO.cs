using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class ExamResultDTO
    {
        public int Id { get; set; }
        public decimal ExamMarks { get; set; }
        public string StudentStatus { get; set; }

        public int ExamId { get; set; }
        public ExamDTO ExamDTO { get; set; }

        public int StudentId { get; set; }
        public StudentDTO StudentDTO { get; set; }
    }
}
