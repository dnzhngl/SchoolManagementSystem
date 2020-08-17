using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class StudentDetailsViewModel
    {
        public StudentDTO StudentDTO { get; set; }
        public List<StudentDTO> StudentDTOs { get; set; }

        public SectionDTO SectionDTO { get; set; }
        public List<SectionDTO> SectionDTOs { get; set; }

        public GradeDTO GradeDTO { get; set; }
        public List<GradeDTO> GradeDTOs { get; set; }
    }
}
