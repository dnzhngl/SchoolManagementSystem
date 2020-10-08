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
        SubjectDTO GetSubject(string subjectName);
        SubjectDTO GetSubjectIncludes(string subjectName, string sectionName); //
        SubjectDTO GetSubjectIncludes(string subjectName);
        SubjectDTO NewSubject(SubjectDTO subject);
        SubjectDTO UpdateSubject(SubjectDTO subject);
        bool DeleteSubject(int id);
        List<SubjectDTO> GetSubjectByMainSubject(int mainSubjectId);
        //SubjectDTO GetSubjectByName(string subjectName);
        //SubjectDTO GetSubjectIncludeMainSubject(string subjectName);
    }
}
