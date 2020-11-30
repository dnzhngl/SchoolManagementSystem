using SMS.DTO;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class InstructorBranchViewModel
    {
        public InstructorDTO InstructorDTO { get; set; }
        public List<InstructorDTO> InstructorDTOs { get; set; }

        public BranchDTO BranchDTO { get; set; }
        public List<BranchDTO> BranchDTOs { get; set; }

    }
}
