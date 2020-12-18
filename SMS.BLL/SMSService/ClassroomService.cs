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
    public class ClassroomService : IClassroomService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Classroom> classroomRepo;
        public ClassroomService(IUnitOfWork _uow)
        {
            uow = _uow;
            classroomRepo = uow.GetRepository<Classroom>();
        }

        public bool DeleteClassroom(int id)
        {
            try
            {
                var selectedClassroom = classroomRepo.Get(z => z.Id == id);
                classroomRepo.Delete(selectedClassroom);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<ClassroomDTO> GetAll()
        {
            var classroomList = classroomRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<ClassroomDTO>>(classroomList);
        }

        public List<ClassroomDTO> GetClasroomsThatAreFree(int dayId, int lessontimeId)
        {
            var clasroomList = classroomRepo.GetIncludesList(null, z => z.Timetables);
            var cList = clasroomList.Where(z => z.Timetables.Any(x => x.DayId == dayId && x.LessonTimeId == lessontimeId)).Select(x => x.Id);
            var result = clasroomList.Where(z => !cList.Contains(z.Id));
            return MapperFactory.CurrentMapper.Map<List<ClassroomDTO>>(result);
        }

        public ClassroomDTO GetClassroom(int id)
        {
            var selected = classroomRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<ClassroomDTO>(selected);
        }

        public ClassroomDTO GetClassroomByName(string name)
        {
            var classroom = classroomRepo.Get(z => z.ClassroomName == name);
            return MapperFactory.CurrentMapper.Map<ClassroomDTO>(classroom);
        }

        public ClassroomDTO NewClassroom(ClassroomDTO classroom)
        {
            if (!classroomRepo.GetAll().Any( z=> z.ClassroomName.ToLower() == classroom.ClassroomName.ToLower()))
            {
                var newClassroom = MapperFactory.CurrentMapper.Map<Classroom>(classroom);
                newClassroom = classroomRepo.Add(newClassroom);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<ClassroomDTO>(newClassroom);
            }
            else
            {
                return null;
            }
        }

        public ClassroomDTO UpdateClassroom(ClassroomDTO classroom)
        {
            var selected = classroomRepo.Get(z => z.Id == classroom.Id);
            selected = MapperFactory.CurrentMapper.Map<Classroom>(classroom);
            classroomRepo.Update(selected);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<ClassroomDTO>(selected);
        }
    }
}
