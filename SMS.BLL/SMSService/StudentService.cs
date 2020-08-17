using AutoMapper;
using SMS.BLL.Abstract;
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
        public StudentService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                var selectedStudent = uow.GetRepository<Student>().Get(z => z.Id == id);
                uow.GetRepository<Student>().Delete(selectedStudent);
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
            var studentList = uow.GetRepository<Student>().GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        public StudentDTO GetStudent(int id)
        {
            var selectedStudent = uow.GetRepository<Student>().Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }

        public List<StudentDTO> GetStudentBySection(int sectionId)
        {
            //var studentList = uow.GetRepository<Student>().GetIncludes(z => z.SectionId == sectionId);
            var studentList = uow.GetRepository<Student>().GetAll().Where(z => z.SectionId == sectionId);
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentList);
        }

        public List<StudentDTO> GetStudentByParent(int parentId)
        {
            var studentsOfParent = uow.GetRepository<Student>().GetAll().Where(z => z.ParentId == parentId);
            return MapperFactory.CurrentMapper.Map<List<StudentDTO>>(studentsOfParent);
        }

        public StudentDTO NewStudent(StudentDTO student)
        {
            if (!uow.GetRepository<Student>().GetAll().Any(z => z.FirstName.ToLower() == student.FirstName.ToLower() && z.LastName.ToLower() == student.LastName.ToLower()))  //Buraya denetleme olarak TC kimlik numarasını ekleyebilirsin. Onun için kayıt esnasında TC iste.
            {

                var newStudent = MapperFactory.CurrentMapper.Map<Student>(student);            
                newStudent = uow.GetRepository<Student>().Add(newStudent);
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
            var selectedStudent = uow.GetRepository<Student>().Get(z => z.Id == student.Id);
            selectedStudent = MapperFactory.CurrentMapper.Map<Student>(selectedStudent);
            selectedStudent.Parent = MapperFactory.CurrentMapper.Map<Parent>(student.ParentDTO);
            selectedStudent.Section = MapperFactory.CurrentMapper.Map<Section>(student.SectionDTO);

            uow.GetRepository<Student>().Update(selectedStudent);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<StudentDTO>(selectedStudent);
        }
    }
}
