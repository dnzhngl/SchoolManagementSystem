
using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IDayService : IServiceBase
    {
        List<DayDTO> GetAll();
        DayDTO GetDay(int id);
        DayDTO GetDayByName(string dayName);
    }
}
