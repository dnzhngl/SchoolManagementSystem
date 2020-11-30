using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IInstructorService : IServiceBase
    {
        List<InstructorDTO> GetAll();
        InstructorDTO GetInstructor(int id);
        InstructorDTO GetInstructoreByUserId(int id);
        InstructorDTO GetInstructorByUsername(string username);
        List<InstructorDTO> GetInstructorName();
        List<InstructorDTO> GetInstructorNameWithBranch();
        InstructorDTO NewInstructor(InstructorDTO instructor);
        InstructorDTO UpdateInstructor(InstructorDTO instructor);
        bool DeleteInstructor(int id);

        List<InstructorDTO> GetAllInstructorsBasedOnBranch(int branchId);
        InstructorDTO GetInstructorByName(string fullName);
        InstructorDTO GetInstructorByDuty(string duty);
    }
}
