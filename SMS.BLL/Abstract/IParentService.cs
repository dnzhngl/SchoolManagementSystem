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
        ParentDTO GetParent(string identityNo, string firstname, string lastname);
        ParentDTO GetParentByUserId(int userId);
        ParentDTO GetParentWithStudents(int parentId);
        ParentDTO NewParent(ParentDTO parent);
        ParentDTO UpdateParent(ParentDTO parent);
        bool DeleteParent(int id);
        List<ParentDTO> GetInstructorsParents(string instructorUsername);

    }
}
