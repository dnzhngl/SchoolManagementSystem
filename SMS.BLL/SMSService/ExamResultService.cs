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
        private IRepository<Semester> semesterRepo;

        public ExamResultService(IUnitOfWork _uow)
        {
            uow = _uow;
            examResultRepo = uow.GetRepository<ExamResult>();
            examRepo = uow.GetRepository<Exam>();
            subjectRepo = uow.GetRepository<Subject>();
            semesterRepo = uow.GetRepository<Semester>();
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
            var selectedResult = examResultRepo.GetIncludes(z => z.Id == id, z => z.Exam, z => z.Exam.Subject);
            return MapperFactory.CurrentMapper.Map<ExamResultDTO>(selectedResult);
        }

        public ExamResultDTO NewExamResult(ExamResultDTO examResult)
        {
            if (!examResultRepo.GetAll().Any(z => z.StudentId == examResult.StudentId && z.ExamId == examResult.ExamId))
            {
                ExamResult newExamResult = MapperFactory.CurrentMapper.Map<ExamResult>(examResult);

                GetStudentStatus(newExamResult);

                examResultRepo.Add(newExamResult);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<ExamResultDTO>(newExamResult);
            }
            else
            {
                return null;
            }
        }

        private static void GetStudentStatus(ExamResult ExamResult)
        {
            if (ExamResult.ExamMark >= 85 && ExamResult.ExamMark <= 100)
            {
                ExamResult.StudentStatus = "Pekiyi";
                ExamResult.ExamMarkNumeral = 5;
            }
            else if (ExamResult.ExamMark >= 70 && ExamResult.ExamMark < 85)
            {
                ExamResult.StudentStatus = "İyi";
                ExamResult.ExamMarkNumeral = 4;
            }
            else if (ExamResult.ExamMark >= 55 && ExamResult.ExamMark < 70)
            {
                ExamResult.StudentStatus = "Orta";
                ExamResult.ExamMarkNumeral = 3;
            }
            else if (ExamResult.ExamMark >= 45 && ExamResult.ExamMark < 55)
            {
                ExamResult.StudentStatus = "Geçer";
                ExamResult.ExamMarkNumeral = 2;
            }
            else if (ExamResult.ExamMark >= 25 && ExamResult.ExamMark < 45)
            {
                ExamResult.StudentStatus = "Geçmez";
                ExamResult.ExamMarkNumeral = 1;
            }
            else if (ExamResult.ExamMark >= 0 && ExamResult.ExamMark < 25)
            {
                ExamResult.StudentStatus = "Etkisiz";
                ExamResult.ExamMarkNumeral = 0;
            }
        }

        public ExamResultDTO UpdateExamResult(ExamResultDTO examResult)
        {
            var selectedResult = examResultRepo.Get(z => z.Id == examResult.Id);
            selectedResult = MapperFactory.CurrentMapper.Map<ExamResult>(examResult);

            GetStudentStatus(selectedResult);

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
            var examResults = examResultRepo.GetIncludesList(z => z.StudentId == studentId, z => z.Exam, z => z.Student).ToList();
            return MapperFactory.CurrentMapper.Map<List<ExamResultDTO>>(examResults);
        }

        public List<ExamResultDTO> GetExamResultsOfStudentBasedOnSemester(int studentId, int semesterId)
        {
            var selectedSemester = semesterRepo.Get(z => z.Id == semesterId);
            var results = examResultRepo.GetIncludesList(z => z.StudentId == studentId & z.Exam.ExamDate < selectedSemester.SemesterEnd && z.Exam.ExamDate > selectedSemester.SemesterBeginning, z => z.Exam);
            return MapperFactory.CurrentMapper.Map<List<ExamResultDTO>>(results);
        }
    }
}
