using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IBranchService : IServiceBase
    {
        List<BranchDTO> GetAll();
        BranchDTO GetBranch(int id);
        BranchDTO NewBranch(BranchDTO branch);
        BranchDTO UpdateBranch(BranchDTO branch);
        bool DeleteBranch(int id);
    }
}
