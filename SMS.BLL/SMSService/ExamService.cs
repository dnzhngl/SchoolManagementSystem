using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DAL;
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
        private IRepository<ExamResult> examResultRepo;
        public ExamService(IUnitOfWork _uow)
        {
            uow = _uow;
            examRepo = uow.GetRepository<Exam>();
            examResultRepo = uow.GetRepository<ExamResult>();
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
            if (!examRepo.GetAll().Any(z => z.ExamName == exam.ExamName && z.SubjectId == exam.SubjectId))
            {
                Exam newExam = MapperFactory.CurrentMapper.Map<Exam>(exam);
                examRepo.Add(newExam);
                uow.SaveChanges();

                var addedExam = examRepo.Get(z => z.Id == newExam.Id);
                GenerateExamResults(exam.SubjectId, addedExam.Id);

                return MapperFactory.CurrentMapper.Map<ExamDTO>(newExam);
            }
            else
            {
                return null;
            }
        }
        public void GenerateExamResults(int subjectId, int examId)
        {
            var sections = uow.GetRepository<Timetable>().GetAll().Where(z => z.SubjectId == subjectId).GroupBy(z => z.SectionId).Select(z => z.Key);
            List<Student> students = new List<Student>();
            foreach (var sectionId in sections)
            {
                var studentList = uow.GetRepository<Student>().GetAll().Where(z => z.SectionId == sectionId);
                students.AddRange(studentList);
            }

            foreach (var student in students)
            {
                ExamResult newExamResult = new ExamResult();
                newExamResult.StudentId = student.Id;
                newExamResult.ExamId = examId;

                examResultRepo.Add(newExamResult);
                uow.SaveChanges();
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

        //public List<ExamDTO> StudentsExamList(int studentId)
        //{
        //    var studentsSection = uow.GetRepository<Student>().Get(Z => Z.Id == studentId).SectionId;
        //    var examList = uow.GetRepository<Timetable>().GetIncludesList(z => z.SectionId == studentsSection, z => z.Subject.Exams).Select(z => z.Subject.Exams);
            
        //    return MapperFactory.CurrentMapper.Map<List<ExamDTO>>(examList);
        //}

        public List<ExamDTO> GetExamsByStudent(int studentId)
        {

            var examList = examResultRepo.GetIncludesList(z => z.StudentId == studentId, z => z.Exam).Select(z=> z.Exam).ToList();
            
            return MapperFactory.CurrentMapper.Map<List<ExamDTO>>(examList);
        }
    }
}
