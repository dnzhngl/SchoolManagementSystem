using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IAdminService : IServiceBase
    {
        List<AdminDTO> GetAll();
        AdminDTO GetAdmin(int id);
        AdminDTO NewAdmin(AdminDTO admin);
        AdminDTO UpdateAdmin(AdminDTO admin);
        bool DeleteAdmin(int id);
    }
}
