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
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Exam> examRepo;
        public ExamService(IUnitOfWork _uow)
        {
            uow = _uow;
            examRepo = uow.GetRepository<Exam>();
        }

        public bool DeleteExam(int id)
        {
            try
            {
                var selectedExam = examRepo.Get(z => z.Id == id);
                examRepo.Delete(selectedExam);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ExamDTO> GetAll()
        {
            var examList = examRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<ExamDTO>>(examList);
        }

        public ExamDTO GetExam(int id)
        {
            var selectedExam = examRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<ExamDTO>(selectedExam);
        }

        public List<ExamDTO> GetExamBySubject(int id)
        {
            var examList = examRepo.GetAll().Where(z => z.SubjectId == id);
            return MapperFactory.CurrentMapper.Map<List<ExamDTO>>(examList);
        }

        public ExamDTO NewExam(ExamDTO exam)
        {
            if (!examRepo.GetAll().Any(z => z.ExamName == exam.ExamName))
            {
                Exam newExam = MapperFactory.CurrentMapper.Map<Exam>(exam);
                examRepo.Add(newExam);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<ExamDTO>(newExam);
            }
            else
            {
                return null;
            }
        }

        public ExamDTO UpdateExam(ExamDTO exam)
        {
            var selectedExam = examRepo.Get(z => z.Id == exam.Id);
            selectedExam = MapperFactory.CurrentMapper.Map<Exam>(exam);
            examRepo.Update(selectedExam);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<ExamDTO>(selectedExam);
        }
    }
}
