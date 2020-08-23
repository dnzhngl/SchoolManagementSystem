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
    public class LessonTimeService : ILessonTimeService
    {
        private readonly IUnitOfWork uow;
        private IRepository<LessonTime> lessonTimeRepo;
        public LessonTimeService(IUnitOfWork _uow)
        {
            uow = _uow;
            lessonTimeRepo = uow.GetRepository<LessonTime>();
        }
        public bool DeleteLessonTime(int id)
        {
            try
            {
                var selectedLessonTime = lessonTimeRepo.Get(z => z.Id == id);
                lessonTimeRepo.Delete(selectedLessonTime);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<LessonTimeDTO> GetAll()
        {
            var lessonTimeList = lessonTimeRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<LessonTimeDTO>>(lessonTimeList);
        }

        public LessonTimeDTO GetLessonTime(int id)
        {
            var selectedLessonTime = lessonTimeRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<LessonTimeDTO>(selectedLessonTime);
        }

        public LessonTimeDTO NewLessonTime(LessonTimeDTO lessonTime)
        {
            if (!lessonTimeRepo.GetAll().Any(z => z.LessonBeginTime == lessonTime.LessonBeginTime && z.LessonEndTime == lessonTime.LessonEndTime))
            {
                LessonTime newLessonTime = MapperFactory.CurrentMapper.Map<LessonTime>(lessonTime);
                lessonTimeRepo.Add(newLessonTime);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<LessonTimeDTO>(newLessonTime);
            }
            else
            {
                return null;
            }
        }

        public LessonTimeDTO UpdateLessonTime(LessonTimeDTO lessonTime)
        {
            var selectedLessonTime = lessonTimeRepo.Get(z => z.Id == lessonTime.Id);
            selectedLessonTime = MapperFactory.CurrentMapper.Map<LessonTime>(lessonTime);
            lessonTimeRepo.Update(selectedLessonTime);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<LessonTimeDTO>(selectedLessonTime);
        }
    }
}
