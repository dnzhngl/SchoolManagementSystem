using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IStudentService : IServiceBase
    {
        List<StudentDTO> GetAll();
        List<StudentDTO> GetAllStudents();
        StudentDTO GetStudent(int id);
        StudentDTO GetStudentByUsername(string username);
        List<StudentDTO> GetStudentByParent(int parentId);
        List<StudentDTO> GetStudentBySection(int sectionId);
        List<StudentDTO> GetStudentsByGrade(int gradeId);
        List<StudentDTO> GetStudentBasedOnCertificates(int certificateTypeId);
        List<StudentDTO> GetStudentsOfInstructor(int instructorId);
        List<StudentDTO> GetStudentList(string studentStatus = null);
        List<StudentDTO> GetStudentsIncludes(int sectionId);
        
        StudentDTO NewStudent(StudentDTO student);
        StudentDTO UpdateStudent(StudentDTO student);
        bool DeleteStudent(int id);
        string GenerateStudentNumber();
    }
}
