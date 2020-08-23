using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class LessonTimeDTO
    {
        public int Id { get; set; }
        public string LessonBeginTime { get; set; }
        public string LessonEndTime { get; set; }
        public List<TimeTableDTO> TimeTableDTOs { get; set; }
    }
}
