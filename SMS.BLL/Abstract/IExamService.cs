using SMS.Core.Services;
using SMS.DTO;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IExamService : IServiceBase
    {
        List<ExamDTO> GetAll();
        ExamDTO GetExam(int id);
        List<ExamDTO> GetExamBySubject(int id);
        ExamDTO NewExam(ExamDTO exam);
        ExamDTO UpdateExam(ExamDTO exam);
        bool DeleteExam(int id);
    }
}
