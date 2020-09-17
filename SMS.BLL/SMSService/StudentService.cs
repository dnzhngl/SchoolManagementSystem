using AutoMapper;
using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Student> studentRepo;

        private IRepository<Instructor> instructorRepo;
        private IRepository<TimetableView> timetableViewRepo;
        private IRepository<Timetable> timetableRepo;
        public StudentService(IUnitOfWork _uow)
        {
            uow = _uow;
            studentRepo = uow.GetRepository<Student>();
            instructorRepo = uow.GetRepository<Instructor>();
            timetableViewRepo = uow.GetRepository<TimetableView>();
            timetableRepo = uow.GetRepository<Timetable>();
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                var selectedStudent = studentRepo.Get(z => z.Id == id);
                studentRepo.Delete(selectedStudent);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<StudentDTO> GetAll()
        {
            var studentList = studentRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        /// <summary>
        /// Returns all of the students whose section assigned
        /// </summary>
        /// <returns></returns>
        public List<StudentDTO> GetAllStudents()
        {
            var studentList = studentRepo.GetAll().Where(z => z.SectionId != null).ToList();
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        public StudentDTO GetStudent(int id)
        {
            var selectedStudent = studentRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }

        public List<StudentDTO> GetStudentBySection(int sectionId)
        {
            //var studentList = uow.GetRepository<Student>().GetIncludes(z => z.SectionId == sectionId);
            var studentList = studentRepo.GetAll().Where(z => z.SectionId == sectionId);
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        public List<StudentDTO> GetStudentByParent(int parentId)
        {
            var studentsOfParent = studentRepo.GetAll().Where(z => z.ParentId == parentId);
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentsOfParent);
        }

        public StudentDTO NewStudent(StudentDTO student)
        {
            int studentCount = studentRepo.GetAll().Count();
            //Any(z => z.FirstName.ToLower() == student.FirstName.ToLower() && z.LastName.ToLower() == student.LastName.ToLower() && z.DOB == student.DOB)
            if (!studentRepo.GetAll().Any(z => z.IdentityNumber == student.IdentityNumber))
            {
                var newStudent = MapperFactory.CurrentMapper.Map<Student>(student);
                newStudent.SchoolNumber = GenerateStudentNumber(studentCount);
                newStudent.StudentStatus = true;
                newStudent = studentRepo.Add(newStudent);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<StudentDTO>(newStudent);
            }
            else
            {
                return null;
            }

        }

        public StudentDTO UpdateStudent(StudentDTO student)
        {
            var selectedStudent = studentRepo.Get(z => z.Id == student.Id);
            selectedStudent = MapperFactory.CurrentMapper.Map<Student>(student);
            selectedStudent.Parent = MapperFactory.CurrentMapper.Map<Parent>(student.ParentDTO);
            selectedStudent.Section = MapperFactory.CurrentMapper.Map<Section>(student.SectionDTO);

            studentRepo.Update(selectedStudent);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }

        public List<StudentDTO> GetStudentsByInstructor(int instructorId)
        {
            var sectionList = timetableRepo.GetAll().Where(z => z.InstructorId == instructorId).Select(z => z.SectionId);
            List<Student> studentList = new List<Student>();

            foreach (int sectionId in sectionList)
            {
                var sectionsStudentList = studentRepo.GetAll().Where(z => z.SectionId == sectionId).ToList();
                studentList.AddRange(sectionsStudentList);
            }
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        public StudentDTO GetStudentByUsername(string username)
        {
            var selectedStudent = studentRepo.Get(z => z.IdentityNumber == username);
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }

        string GenerateStudentNumber(int StudentCount)
        {
            int registrationOrder = 100 + StudentCount;
            var year = DateTime.Today.Year.ToString();
            year = year.Substring(2,2);
            var studentNumber = string.Format("{0}-{1}", registrationOrder, year);

            return studentNumber;
        }

        public List<StudentDTO> GetStudentsIncludeSectionAttendanceExamResults()
        {
           
            var studentList = studentRepo.GetIncludes(null, x => x.Section, x => x.Attendances, x => x.ExamR );
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }
    }
}
