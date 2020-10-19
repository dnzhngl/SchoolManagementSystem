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
           // var studentList = studentRepo.GetAll().ToList();
            var studentList = studentRepo.GetIncludesList(null, z => z.User).ToList();
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        /// <summary>
        /// Returns all of the students whose section assigned
        /// </summary>
        /// <returns></returns>
        public List<StudentDTO> GetAllStudents()
        {
            //var studentList = studentRepo.GetAll().Where(z => z.SectionId != null).ToList();
            var studentList = studentRepo.GetIncludesList(z => z.SectionId != null, z => z.User).ToList();
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }
        public StudentDTO GetStudentDetails(int id)
        {
            var selectedStudent = studentRepo.GetIncludes(z => z.Id == id, z=> z.Section);
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }
        public StudentDTO GetStudent(int id)
        {
            var selectedStudent = studentRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }

        public List<StudentDTO> GetStudentBySection(int sectionId)
        {
            var studentList = studentRepo.GetIncludesList(z => z.SectionId == sectionId, z => z.Section, z => z.Section.Grade, z=>z.User).ToList();
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }
        public List<StudentDTO> GetStudentsByGrade(int gradeId)
        {
            var studentList = studentRepo.GetIncludesList(z => z.Section.GradeId == gradeId, z => z.Section, z => z.Section.Grade).ToList();
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
                newStudent.StudentStatusBool = true; //Gereksiz.
                newStudent.StudentStatus = "Öğrenci";
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
            var selectedStudent= MapperFactory.CurrentMapper.Map<Student>(student);
          
            studentRepo.Update(selectedStudent);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }

        public List<StudentDTO> GetStudentsOfInstructor(int instructorId)
        {  
            var sectionIdList = timetableRepo.GetAll().Where(z => z.InstructorId == instructorId).GroupBy(z => z.SectionId).Select(z => z.Key);

            List<Student> studentList = new List<Student>();

            foreach (int sectionId in sectionIdList)
            {
                //var sectionsStudentList = studentRepo.GetAll().Where(z => z.SectionId == sectionId).ToList();
                //studentList.AddRange(sectionsStudentList);

                var students = studentRepo.GetIncludesList(z => z.SectionId == sectionId, z => z.Section, z=>z.User); //
                studentList.AddRange(students);
            }


           // studentList = studentList.GroupBy(z => new { z.SchoolNumber, z.FirstName, z.LastName, z.SectionId }).Select(z => new Student() {SchoolNumber = z.Key.SchoolNumber, FirstName = z.Key.FirstName, LastName = z.Key.LastName, SectionId = z.Key.SectionId }).ToList(); //

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

        /// <summary>
        /// It gives List of Students included Section, ExamResults and Attendances.
        /// If sectionId parameter sent it return the List of students in given section.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public List<StudentDTO> GetStudentsIncludes(int sectionId)
        {
            var studentList = studentRepo.GetIncludesList(x => x.SectionId == sectionId, x => x.Section,  x => x.ExamResults, x => x.Attendances, x=>x.User);
            
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        public List<StudentDTO> GetStudentBasedOnCertificates(int certificateTypeId)
        {
            var certificateList = uow.GetRepository<Certificate>().GetIncludesList(z => z.CertificateTypeId == certificateTypeId, z => z.Student, z=>z.Student.Section.Grade);
            var studentList = certificateList.Select(z => z.Student);
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentStatus"></param>
        /// <returns></returns>
        public List<StudentDTO> GetStudentList(string studentStatus = null)
         {
            //var studentList = studentRepo.GetAll().Where(z => z.StudentStatus == studentStatus).ToList();
            var studentList = new List<Student>();
            if (studentStatus != null)
            {
                studentList = studentRepo.GetIncludesList(z => z.StudentStatus == studentStatus, z => z.Section, z=>z.Section.Grade, z=>z.User).ToList();
            }
            else
            {
                studentList = studentRepo.GetIncludesList(z => z.SectionId != null, z => z.Section, z=>z.Section.Grade, z => z.User).ToList();
            }
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }
    }
}
