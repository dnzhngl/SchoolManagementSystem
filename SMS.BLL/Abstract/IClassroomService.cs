using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IClassroomService : IServiceBase
    {
        List<ClassroomDTO> GetAll();
        ClassroomDTO GetClassroom(int id);
        ClassroomDTO NewClassroom(ClassroomDTO classrroom);
        ClassroomDTO UpdateClassroom(ClassroomDTO classrroom);
        bool DeleteClassroom(int id);
        ClassroomDTO GetClassroomByName(string name);

        List<ClassroomDTO> GetClasroomsThatAreFree(int dayId, int lessontimeId);
    }
}
