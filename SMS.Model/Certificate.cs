using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Certificate : Entity<int>
    {
        //public Guid SerialNumber { get; set; }
        public DateTime IssueDate { get; set; }

        [ForeignKey("Semester")]
        public Nullable<int> SemesterId { get; set; }
        public virtual Semester Semester { get; set; }

        [ForeignKey("Student")]
        public Nullable<int> StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("CertificateType")]
        public Nullable<int> CertificateTypeId { get; set; }
        public virtual CertificateType CertificateType { get; set; }
    }
}
