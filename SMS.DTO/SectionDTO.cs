﻿using System;
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

        public Nullable<int> AdvisoryTeacherId { get; set; }
        public InstructorDTO AdvisoryTeacher { get; set; }

        public int GradeId { get; set; }
        public GradeDTO Grade { get; set; }

        public List<StudentDTO> Students { get; set; }
        public List<TimetableDTO> Timetables { get; set; }
    }
}
