using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<InstructorDTO> InstructorDTOs { get; set; }
        public List<StudentDTO> StudentDTOs { get; set; }
        public List<ParentDTO> ParentDTOs { get; set; }
    }
}
