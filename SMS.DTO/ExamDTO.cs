using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class ExamDTO
    {
        public int Id { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public string ExamStartTime { get; set; }
        public string ExamEndTime { get; set; }

        public int ExamTypeId { get; set; }
        public ExamTypeDTO ExamType{ get; set; }

        public int SubjectId { get; set; }
        public SubjectDTO Subject { get; set; }

        public List<ExamResultDTO> ExamResults { get; set; }
    }
}
