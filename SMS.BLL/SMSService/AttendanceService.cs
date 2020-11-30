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
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Attendance> attendanceRepo;

        public AttendanceService(IUnitOfWork _uow)
        {
            uow = _uow;
            attendanceRepo = uow.GetRepository<Attendance>();
        }

        public bool DeleteAttendance(int id)
        {
            try
            {
                var selectedAttendance = attendanceRepo.Get(z => z.Id == id);
                attendanceRepo.Delete(selectedAttendance);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public List<AttendanceDTO> GetAll()
        {
            var attendanceList = attendanceRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<AttendanceDTO>>(attendanceList);

        }

        public AttendanceDTO GetAttendance(int id)
        {
            var selectedAttendance = attendanceRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<AttendanceDTO>(selectedAttendance);

        }

        public List<AttendanceDTO> GetAttendanceOfStudent(int studentId)
        {
            //var attendanceList = attendanceRepo.GetAll().Where(z => z.StudentId == studentId);
            var attendanceList = attendanceRepo.GetIncludesList(z => z.StudentId == studentId, z => z.AttendanceType);
            return MapperFactory.CurrentMapper.Map<List<AttendanceDTO>>(attendanceList);
        }

        public List<AttendanceDTO> GetAttendanceOfStudent(int studentId, int semesterId)
        {
            var attendanceList = attendanceRepo.GetIncludesList(z => z.StudentId == studentId && z.Semester.Id == semesterId, z => z.AttendanceType, z => z.Semester);
            return MapperFactory.CurrentMapper.Map<List<AttendanceDTO>>(attendanceList);

        }

        public AttendanceDTO NewAttendance(AttendanceDTO attendance)
        {
            if (!attendanceRepo.GetAll().Any(z => z.StudentId == attendance.StudentId && z.DateTime == attendance.DateTime))
            {
                var newAttendance = MapperFactory.CurrentMapper.Map<Attendance>(attendance);
                attendanceRepo.Add(newAttendance);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<AttendanceDTO>(newAttendance);
            }
            else
            {
                return null;
            }
        }


        public AttendanceDTO UpdateAttendance(AttendanceDTO attendance)
        {
            var selectedAttendance = attendanceRepo.Get(z => z.Id == attendance.Id);
            selectedAttendance = MapperFactory.CurrentMapper.Map<Attendance>(attendance);
            attendanceRepo.Update(selectedAttendance);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<AttendanceDTO>(selectedAttendance);
        }

    }
}
