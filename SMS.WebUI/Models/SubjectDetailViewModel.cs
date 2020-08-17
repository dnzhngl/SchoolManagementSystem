using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class SubjectDetailViewModel
    {
        public ExamDTO ExamDTO { get; set; }
        public List<ExamDTO> ExamDTOs { get; set; }

        public ExamTypeDTO ExamTypeDTO { get; set; }
        public List<ExamTypeDTO> ExamTypeDTOs { get; set; }

        public SubjectDTO SubjectDTO { get; set; }
        public List<SubjectDTO> SubjectDTOs { get; set; }

        public MainSubjectDTO MainSubjectDTO { get; set; }
        public List<MainSubjectDTO> MainSubjectDTOs { get; set; }
    }
}
