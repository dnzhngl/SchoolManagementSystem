using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IRoleService : IServiceBase
    {
        List<RoleDTO> GetAll();
        RoleDTO GetRole(int Id);
        List<RoleDTO> GetRoleName(string roleName);
        RoleDTO NewRole(RoleDTO role);
        RoleDTO UpdateRole(RoleDTO role);
        bool DeleteRole(int roleId);
    }
}
