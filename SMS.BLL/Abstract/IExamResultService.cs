using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IExamResultService : IServiceBase
    {
        List<ExamResultDTO> GetAll();
        ExamResultDTO GetExamResult(int id);
        List<ExamResultDTO> GetExamResultsOfStudent(int studentId);
        List<ExamResultDTO> GetExamResultsOfExam(int examId);
        ExamResultDTO NewExamResult(ExamResultDTO examResult);
        ExamResultDTO UpdateExamResult(ExamResultDTO examResult);
        bool DeleteExamResult(int id);

        List<ExamResultDTO> GetExamResultsOfStudentBasedOnSemester(int studentId, int semesterId);

        // IQueryable<object> CreateSchoolReport(int studentId);
    }
}
