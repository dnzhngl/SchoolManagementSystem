using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class AttendanceDTO
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public int StudentId { get; set; }
        public StudentDTO Student { get; set; }

        public int AttendanceTypeId { get; set; }
        public AttendanceTypeDTO AttendanceTypeDTO { get; set; }

    }
}
