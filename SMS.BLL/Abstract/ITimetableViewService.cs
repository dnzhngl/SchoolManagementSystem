using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ITimetableViewService : IServiceBase
    {
        List<TimetableViewDTO> GetAll();
        TimetableViewDTO GetTimeTable(int id);
        List<TimetableViewDTO> GetTimetableBySection(int id);
        TimetableViewDTO NewTimeTable(TimetableViewDTO timeTable);
        TimetableViewDTO UpdateTimeTable(TimetableViewDTO timeTable);
        bool DeleteTimeTable(int id);
    }
}
