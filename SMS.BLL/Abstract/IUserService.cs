using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IUserService : IServiceBase
    {
        List<UserDTO> GetAll();
        UserDTO GetUser(int id);
        UserDTO GetUserByUsername(string userName);
        UserDTO GenerateUserAccount(string firstname, string lastname, string identityNo, string roleName);
        UserDTO NewUser(string identityNo, string roleName);
        UserDTO UpdateUser(UserDTO user);
        bool DeleteUser(int id);

        UserDTO FindWithUsernameAndMail(string mailOrUsername, string password);
        List<UserDTO> GetAllUserInRole(int roleId);
    }
}
