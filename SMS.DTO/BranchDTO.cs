using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public List<InstructorDTO> Instructors { get; set; }
    }
}
