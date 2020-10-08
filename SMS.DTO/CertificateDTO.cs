using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class CertificateDTO
    {
        public int Id { get; set; }
        //public Guid SerialNumber { get; set; }
        public DateTime IssueDate { get; set; }

        public Nullable<int> SemesterId { get; set; }
        public SemesterDTO Semester { get; set; }

        public Nullable<int> StudentId { get; set; }
        public StudentDTO Student { get; set; }

        public Nullable<int> CertificateTypeId { get; set; }
        public CertificateTypeDTO CertificateType { get; set; }
    }
}
