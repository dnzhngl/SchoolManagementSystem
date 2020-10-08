using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.DTO
{
    public class InstructorDTO
    {
        public int Id { get; set; }
        public string IdentityNumber { get; set; }
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

        public Nullable<int> UserId { get; set; }
        public UserDTO UserDTO { get; set; }

        public string FullName { get { return String.Format("{0} {1}", FirstName, LastName); } }
        public string FullNameBranch { get { return String.Format("{0} {1} / {2}", FirstName, LastName, BranchDTO.BranchName); } }
    }
}
