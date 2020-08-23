using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class TimetableViewModel
    {
        public TimeTableDTO TimeTableDTO { get; set; }

        public SectionDTO SectionDTO { get; set; }
        public List<SectionDTO> SectionDTOs { get; set; }

        public SubjectDTO SubjectDTO { get; set; }
        public List<SubjectDTO> SubjectDTOs { get; set; }

        public InstructorDTO InstructorDTO { get; set; }
        public List<InstructorDTO> InstructorDTOs { get; set; }

        public DayDTO DayDTO { get; set; }
        public List<DayDTO> DayDTOs { get; set; }

        public LessonTimeDTO LessonTimeDTO { get; set; }
        public List<LessonTimeDTO> LessonTimeDTOs { get; set; }

        public SemesterDTO SemesterDTO { get; set; }

    }
}
