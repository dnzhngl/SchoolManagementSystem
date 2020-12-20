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
    public class MainSubjectService : IMainSubjectService
    {
        private readonly IUnitOfWork uow;
        private IRepository<MainSubject> mainSubjectRepo;
        private IRepository<Subject> subjectRepo;
        public MainSubjectService(IUnitOfWork _uow)
        {
            uow = _uow;
            mainSubjectRepo = uow.GetRepository<MainSubject>();
            subjectRepo = uow.GetRepository<Subject>();
        }


        public bool DeleteMainSubject(int id)
        {
            try
            {
                var selectedMainSubject = mainSubjectRepo.Get(z => z.Id == id);
                //var selectedMainSubject = mainSubjectRepo.GetIncludes(z => z.Id == id, z=>z.Subjects);
                //mainSubjectRepo.Delete(selectedMainSubject);
                //foreach(var subject in selectedMainSubject.Subjects)
                //{
                //    subjectRepo.Delete(subject);
                //    uow.SaveChanges();
                //}
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<MainSubjectDTO> GetAll()
        {
            var mainSubjectList = mainSubjectRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<MainSubjectDTO>>(mainSubjectList);
        }

        public MainSubjectDTO GetMainSubject(int id)
        {
            var selectedMainSubject = mainSubjectRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<MainSubjectDTO>(selectedMainSubject);
        }

        public MainSubjectDTO NewMainSubject(MainSubjectDTO mainSubject)
        {
            if (!mainSubjectRepo.GetAll().Any(z => z.MainSubjectName.ToLower() == mainSubject.MainSubjectName.ToLower()))
            {
                MainSubject newMainSubject = MapperFactory.CurrentMapper.Map<MainSubject>(mainSubject);
                mainSubjectRepo.Add(newMainSubject);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<MainSubjectDTO>(newMainSubject);
            }
            else
            {
                return null;
            }
        }

        public MainSubjectDTO UpdateMainSubject(MainSubjectDTO mainSubject)
        {
            var selectedMainSubject = mainSubjectRepo.Get(z => z.Id == mainSubject.Id);
            selectedMainSubject = MapperFactory.CurrentMapper.Map<MainSubject>(mainSubject);
            mainSubjectRepo.Update(selectedMainSubject);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<MainSubjectDTO>(selectedMainSubject);
        }
    }
}
