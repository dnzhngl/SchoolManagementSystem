using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Role> roleRepo;
        public RoleService(IUnitOfWork _uow)
        {
            uow = _uow;
            roleRepo = uow.GetRepository<Role>();
        }
        public bool DeleteRole(int roleId)
        {
            try
            {
                Role selectedRole = roleRepo.Get(z => z.Id == roleId);
                roleRepo.Delete(selectedRole);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<RoleDTO> GetAll()
        {
            var roleList = roleRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<RoleDTO>>(roleList);
        }

        public RoleDTO GetRole(int Id)
        {
            var selectedRole = roleRepo.Get(z => z.Id == Id);
            return MapperFactory.CurrentMapper.Map<RoleDTO>(selectedRole);
        }

        public List<RoleDTO> GetRoleName(string roleName)
        {
            var roleList = roleRepo.Get(z => z.RoleName.Contains(roleName), null).ToList();
            return MapperFactory.CurrentMapper.Map<List<RoleDTO>>(roleList);
        }

        public RoleDTO NewRole(RoleDTO role)
        {
            if (!roleRepo.GetAll().Any(z => z.RoleName == role.RoleName))
            {
                var newRole = MapperFactory.CurrentMapper.Map<Role>(role);
                roleRepo.Add(newRole);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<RoleDTO>(newRole);
            }
            else
            {
                return null;
            }
        }

        public RoleDTO UpdateRole(RoleDTO role)
        {
            var selectedRole = roleRepo.Get(z => z.Id == role.Id);
            selectedRole = MapperFactory.CurrentMapper.Map<Role>(role);
            roleRepo.Update(selectedRole);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<RoleDTO>(selectedRole);
        }
    }
}
