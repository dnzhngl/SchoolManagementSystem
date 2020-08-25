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
    public class TimetableService : ITimetableService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Timetable> timeTableRepo;
        public TimetableService(IUnitOfWork _uow)
        {
            uow = _uow;
            timeTableRepo = uow.GetRepository<Timetable>();
        }

        public bool DeleteTimeTable(int id)
        {
            try
            {
                var selectedTimetable = timeTableRepo.Get(z => z.Id == id);
                timeTableRepo.Delete(selectedTimetable);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<TimetableDTO> GetAll()
        {
            var timeTableList = timeTableRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<TimetableDTO>>(timeTableList);
        }

        public TimetableDTO GetTimeTable(int id)
        {
            var selectedTimetable = timeTableRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<TimetableDTO>(selectedTimetable);

        }

        public TimetableDTO NewTimeTable(TimetableDTO timeTable)
        {
            if (!timeTableRepo.GetAll().Any(z => z.ClassroomName == timeTable.ClassroomName && z.DayId == timeTable.DayId && z.LessonTimeId == timeTable.LessonTimeId))
            {
                Timetable newTimetable = MapperFactory.CurrentMapper.Map<Timetable>(timeTable);
                timeTableRepo.Add(newTimetable);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<TimetableDTO>(newTimetable);
            }
            else
            {
                return null;
            }
        }

        public TimetableDTO UpdateTimeTable(TimetableDTO timeTable)
        {
            var selectedTimetable = timeTableRepo.Get(z => z.Id == timeTable.Id);
            selectedTimetable = MapperFactory.CurrentMapper.Map<Timetable>(timeTable);
            timeTableRepo.Update(selectedTimetable);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<TimetableDTO>(selectedTimetable);
        }
    }
}
