using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IParentService : IServiceBase
    {
        List<ParentDTO> GetAll();
        ParentDTO GetParent(int id);
        List<ParentDTO> GetParentName(string name);
        ParentDTO NewParent(ParentDTO parent);
        ParentDTO UpdateParent(ParentDTO parent);
        bool DeleteParent(int id);
    }
}
