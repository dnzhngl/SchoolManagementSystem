﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class LessonTimeDTO
    {
        public int Id { get; set; }
        public string LessonBeginTime { get; set; }
        public string LessonEndTime { get; set; }
        public List<TimetableDTO> TimeTableDTOs { get; set; }

        public string LessonPeriod { get { return String.Format("{0} - {1}", LessonBeginTime, LessonEndTime); } }

    }
}
