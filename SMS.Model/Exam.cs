using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace SMS.Model
{
    public class Exam : Entity<int>
    {
        public Exam()
        {
            ExamResults = new HashSet<ExamResult>();
        }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public string ExamStartTime { get; set; }
        public string ExamEndTime { get; set; }

        [ForeignKey("ExamType")]
        public int ExamTypeId { get; set; }
        public virtual ExamType ExamType { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public virtual ICollection<ExamResult> ExamResults { get; set; }
    }
}
