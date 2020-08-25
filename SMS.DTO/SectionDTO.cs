using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int StudentCapacity { get; set; }
        public int NumberOfStudentsEnrolled { get; set; }


        public int GradeId { get; set; }
        public GradeDTO GradeDTO { get; set; }

        public List<StudentDTO> StudentDTOs { get; set; }
        public List<TimetableDTO> TimeTableDTOs { get; set; }
    }
}
