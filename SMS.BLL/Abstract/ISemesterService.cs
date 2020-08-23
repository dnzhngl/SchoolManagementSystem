using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ISemesterService : IServiceBase
    {
        List<SemesterDTO> GetAll();
        SemesterDTO GetSemesterInfo(int id);
        SemesterDTO NewSemester(SemesterDTO semester);
        SemesterDTO UpdateSemester(SemesterDTO semester);
        bool DeleteSemester(int id);
    }
}
