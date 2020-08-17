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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Subject> subjectRepo;
        public SubjectService(IUnitOfWork _uow)
        {
            uow = _uow;
            subjectRepo = uow.GetRepository<Subject>();
        }

        public bool DeleteSubject(int id)
        {
            try
            {
                var selectedSubject = subjectRepo.Get(z => z.Id == id);
                subjectRepo.Delete(selectedSubject);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SubjectDTO> GetAll()
        {
            var subjectList = subjectRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<SubjectDTO>>(subjectList);
        }

        public SubjectDTO GetSubject(int id)
        {
            var selectedSubject = subjectRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<SubjectDTO>(selectedSubject);
        }

        public List<SubjectDTO> GetSubjectByMainSubject(int mainSubjectId)
        {
            var subjectList = subjectRepo.GetAll().Where(z => z.MainSubjectId == mainSubjectId);
            //var subjectList = subjectRepo.Get(null, x => x.MainSubjectId == mainSubjectId);
            return MapperFactory.CurrentMapper.Map<List<SubjectDTO>>(subjectList);
        }

        public SubjectDTO NewSubject(SubjectDTO subject)
        {
            if (!subjectRepo.GetAll().Any(z => z.SubjectName.ToLower() == subject.SubjectName.ToLower()))
            {
                Subject newSubject = MapperFactory.CurrentMapper.Map<Subject>(subject);
                subjectRepo.Add(newSubject);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<SubjectDTO>(newSubject);
            }
            else
            {
                return null;
            }
        }

        public SubjectDTO UpdateSubject(SubjectDTO subject)
        {
            var selectedSubject = subjectRepo.Get(z => z.Id == subject.Id);
            selectedSubject = MapperFactory.CurrentMapper.Map<Subject>(subject);
            subjectRepo.Update(selectedSubject);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<SubjectDTO>(selectedSubject);
        }
    }
}
