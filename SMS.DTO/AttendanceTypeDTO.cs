using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class AttendanceTypeDTO
    {
        public int Id { get; set; }
        public string AttendanceTypeName { get; set; }
        public string Description { get; set; }
        public List<AttendanceDTO> AttendanceDTOs { get; set; }
    }
}
