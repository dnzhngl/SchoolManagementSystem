using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IExamTypeService : IServiceBase
    {
        List<ExamTypeDTO> GetAll();
        ExamTypeDTO GetExamType(int id);
        
    }
}
