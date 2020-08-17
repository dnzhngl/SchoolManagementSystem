using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class ExamTypeDTO
    {
        public int Id { get; set; }
        public string ExamTypeName { get; set; }
        public List<ExamDTO> ExamDTOs { get; set; }
    }
}
