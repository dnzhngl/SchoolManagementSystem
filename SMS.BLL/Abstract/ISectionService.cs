using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ISectionService : IServiceBase
    {
        List<SectionDTO> GetAll();
        SectionDTO GetSection(int id);
        SectionDTO NewSection(SectionDTO section);
        SectionDTO UpdateSection(SectionDTO section);
        bool DeleteSection(int id);
        List<SectionDTO> GetSectionByGrade(int id);
        SectionDTO GetSectionByName(string sectionName);
    }
}
