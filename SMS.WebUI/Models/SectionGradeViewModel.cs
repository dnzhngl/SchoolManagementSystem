using SMS.DTO;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class SectionGradeViewModel
    {
        public SectionDTO SectionDTO { get; set; }
        public List<SectionDTO> SectionDTOs { get; set; }

        public GradeDTO GradeDTO { get; set; }
        public List<GradeDTO> GradeDTOs { get; set; }

        public InstructorDTO InstructorDTO { get; set; }
        public List<InstructorDTO> InstructorDTOs { get; set; }

    }
}
