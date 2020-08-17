using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IMainSubjectService : IServiceBase
    {
        List<MainSubjectDTO> GetAll();
        MainSubjectDTO GetMainSubject(int id);
        MainSubjectDTO NewMainSubject(MainSubjectDTO mainSubject);
        MainSubjectDTO UpdateMainSubject(MainSubjectDTO mainSubject);
        bool DeleteMainSubject(int id);

    }
}
