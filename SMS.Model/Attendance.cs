using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Attendance : Entity<int>
    {
        public DateTime DateTime { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("AttendanceType")]
        public int AttendanceTypeId { get; set; }
        public AttendanceType AttendanceType { get; set; }

    }
}
