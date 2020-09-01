using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class ClassroomDTO
    {
        public int Id { get; set; }
        public string ClassroomName { get; set; }
        public int StudentCapacity { get; set; }
        public List<TimetableDTO> TimeTableDTOs { get; set; }
    }
}
