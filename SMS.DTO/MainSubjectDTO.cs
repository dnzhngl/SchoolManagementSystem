using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class MainSubjectDTO
    {
        public int Id { get; set; }
        public string MainSubjectName { get; set; }
        public List<SubjectDTO> Subjects { get; set; }
    }
}
