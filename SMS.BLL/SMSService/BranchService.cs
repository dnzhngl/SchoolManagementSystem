using SMS.BLL.Abstract;
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
        public BranchService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public bool DeleteBranch(int id)
        {
            try
            {
                var deleteBranch = uow.GetRepository<Branch>().Get(z => z.Id == id);
                uow.GetRepository<Branch>().Delete(deleteBranch);
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
            var branchList = uow.GetRepository<Branch>().GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<BranchDTO>>(branchList);
        }

        public BranchDTO GetBranch(int id)
        {
            var selectedBranch = uow.GetRepository<Branch>().Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<BranchDTO>(selectedBranch);
        }

        // Burayı henüz yazmadın
        public List<BranchDTO> GetBranchNameList(string branchName)
        {
            throw new NotImplementedException();
        }

        public BranchDTO NewBranch(BranchDTO branch)
        {
            if (!uow.GetRepository<Branch>().GetAll().Any(z => z.BranchName.ToLower() == branch.BranchName.ToLower()))
            {
                var newBranch = MapperFactory.CurrentMapper.Map<Branch>(branch);
                newBranch = uow.GetRepository<Branch>().Add(newBranch);
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
            var updatedBranch = uow.GetRepository<Branch>().Get(z => z.Id == branch.Id);
            updatedBranch = MapperFactory.CurrentMapper.Map<Branch>(branch);
            uow.GetRepository<Branch>().Update(updatedBranch);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<BranchDTO>(updatedBranch);
        }
    }
}
