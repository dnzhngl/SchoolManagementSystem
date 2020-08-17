using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public List<SectionDTO> Sections { get; set; }
    }
}
