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
        /// <summary>
        /// It gives the related subject object according to the id number.
        /// </summary>
        /// <param name="id">Subject Id</param>
        /// <returns>Subject object</returns>
        public SubjectDTO GetSubject(int id)
        {
            var selectedSubject = subjectRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<SubjectDTO>(selectedSubject);
        }

        /// <summary>
        /// It gives the related object according to the subject Name.
        /// </summary>
        /// <param name="subjectName">Subject name</param>
        /// <returns>Subject object</returns>
        public SubjectDTO GetSubject(string subjectName)
        {
            var subject = subjectRepo.Get(z => z.SubjectName == subjectName);
            return MapperFactory.CurrentMapper.Map<SubjectDTO>(subject);
        }

        public List<SubjectDTO> GetSubjectByMainSubject(int mainSubjectId)
        {
            var subjectList = subjectRepo.GetAll().Where(z => z.MainSubjectId == mainSubjectId);
            //var subjectList = subjectRepo.Get(null, x => x.MainSubjectId == mainSubjectId);
            return MapperFactory.CurrentMapper.Map<List<SubjectDTO>>(subjectList);
        }

        /// <summary>
        /// It gives the related object according to the subject name, and includes related table names.
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns>Subject object included related Main subject and Exams</returns>
        public SubjectDTO GetSubjectIncludes(string subjectName)
        {
            var subject = subjectRepo.GetIncludes(z => z.SubjectName == subjectName, z => z.MainSubject, z => z.Exams); //Examı ekledin
            return MapperFactory.CurrentMapper.Map<SubjectDTO>(subject);
        }

        public SubjectDTO GetSubjectIncludes(string subjectName, string sectionName)
        {
            //.SelectMany(z => z.Subject.Exams);
            //var examList = uow.GetRepository<Timetable>().GetIncludesList(z => z.Section.SectionName == sectionName && z.Subject.SubjectName == subjectName, z => z.Subject.Exams).Select(z => z.Subject.Exams);
            //examList = (IQueryable<Exam>)examList.GroupBy(z => z.ExamName).SelectMany(z => z.Key);

            var subject = subjectRepo.GetIncludes(z => z.SubjectName == subjectName && z.Timetables.Any(x => x.Section.SectionName == sectionName), z => z.Exams); //Examı ekledin
            return MapperFactory.CurrentMapper.Map<SubjectDTO>(subject);
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


        //public SubjectDTO GetSubjectByName(string subjectName)
        //{
        //    var subject = subjectRepo.Get(z => z.SubjectName == subjectName);
        //    return MapperFactory.CurrentMapper.Map<SubjectDTO>(subject);
        //}


        //public SubjectDTO GetSubjectIncludeMainSubject(string subjectName)
        //{
        //    var subject = subjectRepo.GetIncludes(z => z.SubjectName == subjectName, z => z.MainSubject, z => z.Exams); //Examı ekledin

        //    return MapperFactory.CurrentMapper.Map<SubjectDTO>(subject);
        //}
    }
}
