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
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Branch> branchRepo;
        public BranchService(IUnitOfWork _uow)
        {
            uow = _uow;
            branchRepo = uow.GetRepository<Branch>();
        }

        public bool DeleteBranch(int id)
        {
            try
            {
                var deleteBranch = branchRepo.Get(z => z.Id == id);
                branchRepo.Delete(deleteBranch);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<BranchDTO> GetAll()
        {
            var branchList = branchRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<BranchDTO>>(branchList);
        }

        public BranchDTO GetBranch(int id)
        {
            var selectedBranch = branchRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<BranchDTO>(selectedBranch);
        }

        public BranchDTO NewBranch(BranchDTO branch)
        {
            if (!branchRepo.GetAll().Any(z => z.BranchName.ToLower() == branch.BranchName.ToLower()))
            {
                var newBranch = MapperFactory.CurrentMapper.Map<Branch>(branch);
                newBranch = branchRepo.Add(newBranch);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<BranchDTO>(newBranch);
            }
            else
            {
                return null;
            }
        }

        public BranchDTO UpdateBranch(BranchDTO branch)
        {
            var updatedBranch = branchRepo.Get(z => z.Id == branch.Id);
            updatedBranch = MapperFactory.CurrentMapper.Map<Branch>(branch);
            branchRepo.Update(updatedBranch);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<BranchDTO>(updatedBranch);
        }
    }
}
