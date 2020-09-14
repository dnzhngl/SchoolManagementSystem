using Microsoft.EntityFrameworkCore.Internal;
using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace SMS.BLL.SMSService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private IRepository<User> userRepo;
        private IRepository<Role> roleRepo;

        public UserService(IUnitOfWork _uow)
        {
            uow = _uow;
            userRepo = uow.GetRepository<User>();
            roleRepo = uow.GetRepository<Role>();
        }
        public bool DeleteUser(int id)
        {
            try
            {
                var selectedUser = userRepo.Get(z => z.Id == id);
                userRepo.Delete(selectedUser);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public UserDTO FindWithUsernameAndMail(string mailOrUsername, string password)
        {
            var user = userRepo.Get(z => (z.UserName == mailOrUsername && z.Password == password));
            return MapperFactory.CurrentMapper.Map<UserDTO>(user);
        }

        public UserDTO GenerateUserAccount(string firstname, string lastname, string identityNo, string roleName)
        {
            User newUser = new User();
            newUser.UserName = CreateUserName(firstname, lastname);
            newUser.Password = identityNo;
            newUser.RoleId = roleRepo.Get(z => z.RoleName == roleName).Id;

            userRepo.Add(newUser);
            uow.SaveChanges();

            return MapperFactory.CurrentMapper.Map<UserDTO>(newUser);
        }

        public List<UserDTO> GetAll()
        {
            var userList = userRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<UserDTO>>(userList);
        }

        public List<UserDTO> GetAllUserInRole(int roleId)
        {
            var userList = userRepo.GetAll().Where(z => z.RoleId == roleId).ToList();
            return MapperFactory.CurrentMapper.Map<List<UserDTO>>(userList);
        }

        public UserDTO GetUser(int id)
        {
            var selectedUser = userRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<UserDTO>(selectedUser);
        }

        public UserDTO GetUserByUsername(string userName)
        {
            var user = userRepo.Get(z => z.UserName == userName);
            return MapperFactory.CurrentMapper.Map<UserDTO>(user);
        }

        public UserDTO NewUser(string identityNo, string roleName)
        {
            if (!userRepo.GetAll().Any(z => z.UserName == identityNo))
            {
                User newUser = new User();
                newUser.UserName = identityNo;
                newUser.Password = identityNo;
                newUser.RoleId = roleRepo.Get(z => z.RoleName == roleName).Id;
                userRepo.Add(newUser);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<UserDTO>(newUser);
            }
            else
            {
                return null;
            }

        }

        public UserDTO UpdateUser(UserDTO user)
        {
            var selectedUser = userRepo.Get(z => z.Id == user.Id);
            selectedUser = MapperFactory.CurrentMapper.Map<User>(user);
            userRepo.Update(selectedUser);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<UserDTO>(selectedUser);
        }

        string CreateUserName(string firstname, string lastname)
        {
            var userName = String.Format("{0}.{1}", firstname, lastname);

            if (!userRepo.GetAll().Any(z => z.UserName == userName))
            {
                return userName;
            }
            else
            {
                userName = String.Format("{0}.{1}", lastname, firstname);
                return userName;
            }
        }
    }
}
