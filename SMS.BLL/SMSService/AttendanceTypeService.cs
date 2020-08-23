using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class AttendanceTypeService : IAttendanceTypeService
    {
        private readonly IUnitOfWork uow;
        private IRepository<AttendanceType> attendanceTypeRepo;
        public AttendanceTypeService(IUnitOfWork _uow)
        {
            uow = _uow;
            attendanceTypeRepo = uow.GetRepository<AttendanceType>();
        }
        public List<AttendanceTypeDTO> GetAll()
        {
            var attendanceTypeList = attendanceTypeRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<AttendanceTypeDTO>>(attendanceTypeList);
        }

        public AttendanceTypeDTO GetAttendanceType(int id)
        {
            var selectedAttendanceType = attendanceTypeRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<AttendanceTypeDTO>(selectedAttendanceType);
        }
    }
}
