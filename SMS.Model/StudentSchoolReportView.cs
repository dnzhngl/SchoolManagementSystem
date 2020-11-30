using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class StudentSchoolReportView : Entity<int>
    {
        public string SchoolNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubjectName { get; set; }
        public decimal AvgMark { get; set; }
        public int AvgMarkNumeral { get; set; }
        public decimal GPA { get; set; }
        public int GpaNumeral { get; set; }
        public int NumberOfExams { get; set; }
        public int WeeklyCourseHours { get; set; }
        public string MainSubjectName { get; set; }
        public string SemesterName { get; set; }
        public string AcademicYear { get; set; }

    }
}
