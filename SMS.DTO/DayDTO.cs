using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class DayDTO
    {
        public int Id { get; set; }
        public string DayName { get; set; }
        public List<TimeTableDTO> TimeTableDTOs { get; set; }
    }
}
