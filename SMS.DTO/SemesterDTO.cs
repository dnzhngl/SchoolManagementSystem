using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class SemesterDTO
    {
        public int Id { get; set; }
        public DateTime SemesterBeginning { get; set; }
        public DateTime SemesterEnd { get; set; }

        public int TimeTableId { get; set; }
        public List<TimeTableDTO> TimeTableDTOs { get; set; }
    }
}
