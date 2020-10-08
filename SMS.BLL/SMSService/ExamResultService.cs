﻿using SMS.BLL.Abstract;
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

        public List<ExamResultDTO> GetExamResultsOfStudent(int studentId)
        {
           // var student = uow.GetRepository<Student>().Get(z => z.Id == studentId);
            var examResults = examResultRepo.GetIncludesList(z => z.StudentId == studentId, z => z.Exam).OrderBy(z => z.Exam.ExamDate).ToList();
            return MapperFactory.CurrentMapper.Map<List<ExamResultDTO>>(examResults);
        }

        public List<ExamResultDTO> GetExamResultBySubject(int id)
        {
            var subject = subjectRepo.Get(z => z.Id == id);
            var subjectsExams = examRepo.GetAll().Where(z => z.SubjectId == id).ToList();
            var examResultList = examResultRepo.GetAll().Where(z => z.ExamId == subjectsExams.FirstOrDefault().Id).ToList();

            return MapperFactory.CurrentMapper.Map<List<ExamResultDTO>>(examResultList);
        }

        public ExamResultDTO NewExamResult(ExamResultDTO examResult)
        {
            if (!examResultRepo.GetAll().Any(z => z.Id == examResult.Id))
            {
                ExamResult newExamResult = MapperFactory.CurrentMapper.Map<ExamResult>(examResult);
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
    }
}
