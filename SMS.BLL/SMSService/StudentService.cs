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
        private IRepository<Role> roleRepo;
        public StudentService(IUnitOfWork _uow)
        {
            uow = _uow;
            studentRepo = uow.GetRepository<Student>();
            roleRepo = uow.GetRepository<Role>();
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
            if (!studentRepo.GetAll().Any(z => z.FirstName.ToLower() == student.FirstName.ToLower() && z.LastName.ToLower() == student.LastName.ToLower()))  //Buraya denetleme olarak TC kimlik numarasını ekleyebilirsin. Onun için kayıt esnasında TC iste.
            {

                var newStudent = MapperFactory.CurrentMapper.Map<Student>(student);
                newStudent.RoleId = roleRepo.Get(z => z.RoleName.Contains("Öğrenci")).Id;
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
    }
}
