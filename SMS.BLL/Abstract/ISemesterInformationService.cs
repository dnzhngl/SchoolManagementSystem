using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ISemesterInformationService : IServiceBase
    {
        List<SemesterInformationDTO> GetAll();
        SemesterInformationDTO GetSemesterInfo(int id);
        SemesterInformationDTO NewSemester(SemesterInformationDTO semester);
        SemesterInformationDTO UpdateSemester(SemesterInformationDTO semester);
        bool DeleteSemester(int id);
    }
}
