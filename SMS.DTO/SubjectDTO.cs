using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int WeeklyCourseHours { get; set; }

        public int MainSubjectId { get; set; }
        public MainSubjectDTO MainSubject { get; set; }
        public List<ExamDTO> Exams { get; set; }
        public List<TimetableDTO> TimeTables { get; set; }
    }
}
