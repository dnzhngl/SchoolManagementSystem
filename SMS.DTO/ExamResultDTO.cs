using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class ExamResultDTO
    {
        public int Id { get; set; }
        public decimal ExamMark { get; set; }
        public string StudentStatus { get; set; }
        public int CreatedBy { get; set; }


        public int ExamId { get; set; }
        public ExamDTO Exam { get; set; }

        public int StudentId { get; set; }
        public StudentDTO Student { get; set; }
    }
}
