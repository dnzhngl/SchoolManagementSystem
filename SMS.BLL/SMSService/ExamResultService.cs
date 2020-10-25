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
    public class ExamResultService : IExamResultService
    {
        private readonly IUnitOfWork uow;
        private IRepository<ExamResult> examResultRepo;
        private IRepository<Exam> examRepo;
        private IRepository<Subject> subjectRepo;
        public ExamResultService(IUnitOfWork _uow)
        {
            uow = _uow;
            examResultRepo = uow.GetRepository<ExamResult>();
            examRepo = uow.GetRepository<Exam>();
            subjectRepo = uow.GetRepository<Subject>();
        }
        public bool DeleteExamResult(int id)
        {
            try
            {
                var selectedResult = examResultRepo.Get(z => z.Id == id);
                examResultRepo.Delete(selectedResult);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ExamResultDTO> GetAll()
        {
            var resultList = examResultRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<ExamResultDTO>>(resultList);
        }

        public ExamResultDTO GetExamResult(int id)
        {
            var selectedResult = examResultRepo.GetIncludes(z => z.Id == id, z => z.Exam, z=>z.Exam.Subject);
            return MapperFactory.CurrentMapper.Map<ExamResultDTO>(selectedResult);
        }

        public ExamResultDTO NewExamResult(ExamResultDTO examResult)
        {
            if (!examResultRepo.GetAll().Any(z => z.StudentId == examResult.StudentId && z.ExamId == examResult.ExamId))
            {
                ExamResult newExamResult = MapperFactory.CurrentMapper.Map<ExamResult>(examResult);

                if (newExamResult.ExamMark >= 85 && newExamResult.ExamMark <= 100)
                {
                    newExamResult.StudentStatus = "Pekiyi";
                }
                else if (newExamResult.ExamMark >= 70 && newExamResult.ExamMark < 85)
                {
                    newExamResult.StudentStatus = "İyi";
                }
                else if (newExamResult.ExamMark >= 60 && newExamResult.ExamMark < 70)
                {
                    newExamResult.StudentStatus = "Orta";
                }
                else if (newExamResult.ExamMark >= 50 && newExamResult.ExamMark < 60)
                {
                    newExamResult.StudentStatus = "Geçer";
                }
                else if (newExamResult.ExamMark >= 0 && newExamResult.ExamMark < 50)
                {
                    newExamResult.StudentStatus = "Geçmez";
                }

                examResultRepo.Add(newExamResult);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<ExamResultDTO>(newExamResult);
            }
            else
            {
                return null;
            }
        }

        public ExamResultDTO UpdateExamResult(ExamResultDTO examResult)
        {
            var selectedResult = examResultRepo.Get(z => z.Id == examResult.Id);
            selectedResult = MapperFactory.CurrentMapper.Map<ExamResult>(examResult);

            if (selectedResult.ExamMark >= 85 && selectedResult.ExamMark <= 100)
            {
                selectedResult.StudentStatus = "Pekiyi";
            }
            else if (selectedResult.ExamMark >= 70 && selectedResult.ExamMark < 85)
            {
                selectedResult.StudentStatus = "İyi";
            }
            else if (selectedResult.ExamMark >= 60 && selectedResult.ExamMark < 70)
            {
                selectedResult.StudentStatus = "Orta";
            }
            else if (selectedResult.ExamMark >= 50 && selectedResult.ExamMark < 60)
            {
                selectedResult.StudentStatus = "Geçer";
            }
            else if (selectedResult.ExamMark >= 0 && selectedResult.ExamMark < 50)
            {
                selectedResult.StudentStatus = "Geçmez";
            }

            examResultRepo.Update(selectedResult);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<ExamResultDTO>(selectedResult);
        }

        public List<ExamResultDTO> GetExamResultsOfExam(int examId)
        {
            var examResultList = examResultRepo.GetIncludesList(z => z.ExamId == examId, z => z.Student).ToList();
            return MapperFactory.CurrentMapper.Map<List<ExamResultDTO>>(examResultList);
        }
        
        public List<ExamResultDTO> GetExamResultsOfStudent(int studentId)
        {
            var examResults = examResultRepo.GetIncludesList(z => z.StudentId == studentId, z => z.Exam, z=>z.Student).ToList();
            return MapperFactory.CurrentMapper.Map<List<ExamResultDTO>>(examResults);
        }
    }
}
