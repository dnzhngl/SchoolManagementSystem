using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;

namespace SMS.WebUI.Controllers
{

    public class BranchController : Controller
    {
        private readonly IBranchService branchService;
        public BranchController(IBranchService _branchService)
        {
            branchService = _branchService;
        }

        public IActionResult BranchList()
        {
            return View(branchService.GetAll());
        }

        public IActionResult BranchAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult BranchAdd(BranchDTO branchDTO)
        {
            branchService.NewBranch(branchDTO);
            return RedirectToAction("BranchList");
        }
        public IActionResult BranchDelete(int id)
        {
            branchService.DeleteBranch(id);
            return RedirectToAction("BranchList");
        }

        public IActionResult BranchUpdate(int id)
        {
            BranchDTO selectedBranch = branchService.GetBranch(id);
            return PartialView(selectedBranch);
        }
        [HttpPost]
        public IActionResult BranchUpdate(BranchDTO branchDTO)
        {
            branchService.UpdateBranch(branchDTO);
            return RedirectToAction("BranchList");
        }
    }
}
