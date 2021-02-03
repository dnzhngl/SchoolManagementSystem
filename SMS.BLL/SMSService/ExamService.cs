using Microsoft.EntityFrameworkCore;
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
            var selectedExam = examRepo.GetIncludes(z => z.Id == id, z => z.Subject, z => z.ExamType);
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
                // GenerateExamResults(exam.SubjectId, addedExam.Id);

                return MapperFactory.CurrentMapper.Map<ExamDTO>(newExam);
            }
            else
            {
                return null;
            }
        }
        public void GenerateExamResults(int subjectId, int examId)
        {
            // var instructors = uow.GetRepository<Timetable>().GetIncludesList(z => z.SubjectId == subjectId, z => z.Instructor, z => z.Section.Students);

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

        public List<ExamDTO> GetExamsByStudent(int studentId)
        {
            var timetables = uow.GetRepository<Student>().GetIncludes(z => z.Id == studentId, z => z.Section.Timetables).Section.Timetables.GroupBy(z => z.SubjectId).Select(z => z.Key);
            var examList = examRepo.GetAll();

            var query = from tt in timetables
                        from exam in examList.Where(exam => exam.SubjectId == tt)
                        select new { exam };

            List<Exam> exams = query.Select(z => z.exam).ToList();

            //Foreach döngüsünü sonradan ekledin.s
            foreach (Exam exam in exams)
            {
                exam.ExamResults = examResultRepo.GetAll().Where(z => z.StudentId == studentId && z.ExamId == exam.Id).ToList();
            }

            return MapperFactory.CurrentMapper.Map<List<ExamDTO>>(exams);
        }
        public List<ExamDTO> GetExamsOfStudent(int studentId)
        {

            var sectionId = uow.GetRepository<Student>().Get(z => z.Id == studentId).SectionId;
            var exams = uow.GetRepository<Timetable>().GetIncludesList(z => z.SectionId == sectionId, z => z.Subject.Exams).SelectMany(z => z.Subject.Exams);
            var examList = exams.GroupBy(z => new Exam() { Id = z.Id, ExamName = z.ExamName, ExamDate = z.ExamDate, ExamTypeId = z.ExamTypeId, SubjectId = z.SubjectId, ExamStartTime = z.ExamStartTime, ExamEndTime = z.ExamEndTime }).Select(z => z.Key);


            return MapperFactory.CurrentMapper.Map<List<ExamDTO>>(examList);

            // select Students.FirstName, Students.LastName, Sections.SectionName, Subjects.SubjectName, Exams.ExamName, Exams.ExamDate, ExamResults.ExamMark from Students
            // inner join Sections on Students.SectionId = Sections.Id
            // inner join Timetables on Sections.Id = Timetables.SectionId
            // inner join Subjects on Timetables.SubjectId = Subjects.Id
            // inner join Exams on Subjects.Id = Exams.SubjectId
            // inner join ExamResults on Exams.Id = ExamResults.ExamId and Students.Id = ExamResults.StudentId
            // Group by  Students.FirstName, Students.LastName, Sections.SectionName, Subjects.SubjectName, Exams.ExamName, Exams.ExamDate, ExamResults.ExamMark
        }

    }
}
