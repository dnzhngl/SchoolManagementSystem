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
        StudentDTO GetStudent(int id);
        List<StudentDTO> GetStudentByParent(int parentId);

        List<StudentDTO> GetStudentBySection(int sectionId);

        StudentDTO NewStudent(StudentDTO student);
        StudentDTO UpdateStudent(StudentDTO student);
        bool DeleteStudent(int id);

    }
}
