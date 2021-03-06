﻿using SMS.BLL.Abstract;
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
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Admin> adminRepo;

        public AdminService(IUnitOfWork _uow)
        {
            uow = _uow;
            adminRepo = uow.GetRepository<Admin>();
        }
        public bool DeleteAdmin(int id)
        {
            try
            {
                var selectedAdmin = adminRepo.Get(z => z.Id == id);
                adminRepo.Delete(selectedAdmin);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AdminDTO GetAdmin(int id)
        {
            var selectedAdmin = adminRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<AdminDTO>(selectedAdmin);
        }

        public List<AdminDTO> GetAll()
        {
           // var adminList = adminRepo.GetAll().ToList();
            var adminList = adminRepo.GetIncludesList(null, z => z.User).ToList();
            return MapperFactory.CurrentMapper.Map<List<AdminDTO>>(adminList);
        }

        public AdminDTO NewAdmin(AdminDTO admin)
        {
            if (!adminRepo.GetAll().Any(z => z.IdentityNumber == admin.IdentityNumber))  
            {
                var newAdmin = MapperFactory.CurrentMapper.Map<Admin>(admin);
                adminRepo.Add(newAdmin);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<AdminDTO>(newAdmin);
            }
            else
            {
                return null;
            }
        }

        public AdminDTO UpdateAdmin(AdminDTO admin)
        {
            var selectedAdmin =  adminRepo.Get(z => z.Id == admin.Id);
            selectedAdmin = MapperFactory.CurrentMapper.Map<Admin>(admin);
            adminRepo.Update(selectedAdmin);
            uow.SaveChanges();

            return MapperFactory.CurrentMapper.Map<AdminDTO>(selectedAdmin);
        }
    }
}
