using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ISubjectService : IServiceBase
    {
        List<SubjectDTO> GetAll();
        SubjectDTO GetSubject(int id);
        SubjectDTO NewSubject(SubjectDTO subject);
        SubjectDTO UpdateSubject(SubjectDTO subject);
        bool DeleteSubject(int id);
        List<SubjectDTO> GetSubjectByMainSubject(int mainSubjectId);

    }
}
