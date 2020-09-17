using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class AttendanceType : Entity<int>
    {
        public AttendanceType()
        {
            Attendances = new HashSet<Attendance>();
        }
        public string AttendanceTypeName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
