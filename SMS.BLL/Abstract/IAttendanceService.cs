using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IAttendanceService : IServiceBase
    {
        List<AttendanceDTO> GetAll();
        AttendanceDTO GetAttendance(int id);
        List<AttendanceDTO> GetAttendanceOfStudent(int id);
        AttendanceDTO NewAttendance(AttendanceDTO attendance);
        AttendanceDTO UpdateAttendance(AttendanceDTO attendance);
        bool DeleteAttendance(int id);
    }
}
