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
    public class TimetableViewService : ITimetableViewService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<TimetableView> ttViewRepo;
        private IRepository<Timetable> timetableRepo;
        private IRepository<Section> sectionRepo;
        private IRepository<Instructor> instructorRepo;
        private IRepository<Classroom> classroomRepo;
        public TimetableViewService(IUnitOfWork _uow)
        {
            uow = _uow;
            ttViewRepo = uow.GetRepository<TimetableView>();
            timetableRepo = uow.GetRepository<Timetable>();
            sectionRepo = uow.GetRepository<Section>();
            instructorRepo = uow.GetRepository<Instructor>();
            classroomRepo = uow.GetRepository<Classroom>();
        }

        public bool DeleteTimeTable(int id)
        {
            try
            {
                var selectedTt = ttViewRepo.Get(z => z.Id == id);
                ttViewRepo.Delete(selectedTt);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<TimetableViewDTO> GetAll()
        {
            var ttList = ttViewRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(ttList);
        }

        public TimetableViewDTO GetTimeTable(int id)
        {
            var selectedTt = ttViewRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<TimetableViewDTO>(selectedTt);

        }

        public List<TimetableViewDTO> GetTimetableGroupedByInstructor(int id)
        {
            var instructor = instructorRepo.Get(z => z.Id == id);
            string instructorFullname = String.Format("{0} {1}", instructor.FirstName, instructor.LastName);
            var ttList = ttViewRepo.GetAll().Where(z => z.Instructor == instructorFullname).ToList();

            var list = ttList.GroupBy(z => new { z.SectionName, z.SubjectName, z.Instructor }).Where(z => z.Key.Instructor == instructorFullname).Select(x => new TimetableView() { SectionName = x.Key.SectionName, SubjectName = x.Key.SubjectName, Instructor = x.Key.Instructor }).ToList();

            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(list);
        }

        public List<TimetableViewDTO> GetTimetableBySection(int id)
        {
            var sectionName = sectionRepo.Get(z => z.Id == id).SectionName;
            //List<TimetableView> ttList = (List<TimetableView>)ttViewRepo.GetAll().Where(z => z.SectionName == sectionName).GroupBy(z => z.SectionName);
            List<TimetableView> ttList = (List<TimetableView>)ttViewRepo.GetAll().Where(z => z.SectionName == sectionName).ToList();
            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(ttList);
        }

        public TimetableViewDTO NewTimeTable(TimetableViewDTO timetable)
        {
            if (!ttViewRepo.GetAll().Any(z => z.Instructor == timetable.Instructor && z.LessonPeriod == timetable.LessonPeriod && z.DayName == timetable.DayName))
            {
                TimetableView newTt = MapperFactory.CurrentMapper.Map<TimetableView>(timetable);
                ttViewRepo.Add(newTt);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<TimetableViewDTO>(newTt);
            }
            else
            {
                return null;
            }
        }

        public TimetableViewDTO UpdateTimeTable(TimetableViewDTO timeTable)
        {
            var selectedTimetable = ttViewRepo.Get(z => z.Id == timeTable.Id);
            selectedTimetable = MapperFactory.CurrentMapper.Map<TimetableView>(timeTable);
            ttViewRepo.Update(selectedTimetable);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<TimetableViewDTO>(selectedTimetable);
        }

        public List<TimetableViewDTO> GetTimetableByInstructor(int id)
        {
            var instructor = instructorRepo.Get(z => z.Id == id);

            string instructorFullname = String.Format("{0} {1}", instructor.FirstName, instructor.LastName);
            var ttList = ttViewRepo.GetAll().Where(z => z.Instructor == instructorFullname).ToList();

            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(ttList);
        }

        public List<TimetableViewDTO> GetTimetableByClassroom(int id)
        {
            var classroom = classroomRepo.Get(z => z.Id == id);
            var classroomTimetable = ttViewRepo.GetAll().Where(z => z.ClassroomName == classroom.ClassroomName).ToList();
            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(classroomTimetable);
        }

    }
}
