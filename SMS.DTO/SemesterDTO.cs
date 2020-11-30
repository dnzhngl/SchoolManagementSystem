using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class SemesterDTO
    {
        public int Id { get; set; }
        public string SemesterName { get; set; }
        public DateTime SemesterBeginning { get ; set; }
        public DateTime SemesterEnd { get; set; }
        public string AcademicYear { get; set; }

        public List<TimetableDTO> Timetables { get; set; }
        public List<CertificateDTO> Certificates { get; set; }

        public string SemesterInfo { get { return String.Format("{0} {1}-{2}", SemesterName, SemesterBeginning.ToString("dd/MM/yyyy"), SemesterEnd.ToString("dd/MM/yyyy")); }}

        //public string AcademicYear { get { return String.Format("{0} / {1}", SemesterBeginning.ToString("yyyy"), SemesterEnd.ToString("yyyy")); }}

    }
}
