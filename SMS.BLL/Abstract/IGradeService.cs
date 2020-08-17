using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IGradeService : IServiceBase
    {
        List<GradeDTO> GetAll();
        GradeDTO GetGrade(int id);
        GradeDTO NewGrade(GradeDTO grade);
        GradeDTO UpdateGrade(GradeDTO grade);
        bool DeleteGrade(int id);

    }
}
