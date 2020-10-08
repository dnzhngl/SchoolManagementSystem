using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IExamResultService : IServiceBase
    {
        List<ExamResultDTO> GetAll();
        ExamResultDTO GetExamResult(int id);
        List<ExamResultDTO> GetExamResultsOfStudent(int studentId);
        List<ExamResultDTO> GetExamResultBySubject(int id);
        ExamResultDTO NewExamResult(ExamResultDTO examResult);
        ExamResultDTO UpdateExamResult(ExamResultDTO examResult);
        bool DeleteExamResult(int id);
    }
}
