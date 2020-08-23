
using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ITimetableService : IServiceBase
    {
        List<TimeTableDTO> GetAll();
        TimeTableDTO GetTimeTable(int id);
        TimeTableDTO NewTimeTable(TimeTableDTO timeTable);
        TimeTableDTO UpdateTimeTable(TimeTableDTO timeTable);
        bool DeleteTimeTable(int id);
    }
}
