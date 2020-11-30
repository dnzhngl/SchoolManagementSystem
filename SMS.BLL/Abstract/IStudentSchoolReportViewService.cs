using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IStudentSchoolReportViewService : IServiceBase
    {
        List<StudentSchoolReportViewDTO> GetAll();
        List<StudentSchoolReportViewDTO> CreateSchoolReport(int studentId);
        IQueryable YearEndSubjectReport(int studentId);
    }
}
