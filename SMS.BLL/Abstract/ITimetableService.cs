
using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ITimetableService : IServiceBase
    {
        List<TimetableDTO> GetAll();
        TimetableDTO GetTimeTable(int id);
        TimetableDTO NewTimeTable(TimetableDTO timeTable);
        TimetableDTO UpdateTimeTable(TimetableDTO timeTable);
        bool DeleteTimeTable(int id);
    }
}
