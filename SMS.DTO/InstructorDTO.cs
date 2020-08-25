using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class InstructorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string Duty { get; set; }

        public int BranchId { get; set; }
        public BranchDTO BranchDTO { get; set; }

        public int TimeTableId { get; set; }
        public List<TimetableDTO> TimeTableDTOs { get; set; }
    }
}
