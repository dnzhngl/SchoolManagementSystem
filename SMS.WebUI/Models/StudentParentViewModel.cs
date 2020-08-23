using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class StudentParentViewModel
    {
        public StudentDTO StudentDTO { get; set; }
        public List<StudentDTO> StudentDTOs { get; set; }
        public ParentDTO ParentDTO { get; set; }
        public List<ParentDTO> ParentDTOs { get; set; }

        public List<SectionDTO> SectionDTOs { get; set; }
    }
}
