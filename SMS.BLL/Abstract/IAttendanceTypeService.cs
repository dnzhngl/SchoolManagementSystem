using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IAttendanceTypeService : IServiceBase
    {
        List<AttendanceTypeDTO> GetAll();
        AttendanceTypeDTO GetAttendanceType(int id);
    }
}
