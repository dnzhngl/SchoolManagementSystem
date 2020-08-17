using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }

        public string GraduatedFrom { get; set; } //Mezun olduğu okul
        public decimal GPA { get; set; }    //Not ortalaması

        public int? SectionId { get; set; }
        public SectionDTO SectionDTO { get; set; }

        public int ParentId { get; set; }
        public ParentDTO ParentDTO { get; set; }

        public List<AttendanceDTO> AttendanceDTOs { get; set; }

    }
}
