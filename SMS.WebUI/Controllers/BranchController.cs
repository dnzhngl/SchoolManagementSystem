﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [ValidateAntiForgeryToken]
        public IActionResult BranchAdd(BranchDTO branchDTO)
        {
            if (ModelState.IsValid)
            {
                branchService.NewBranch(branchDTO);
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return RedirectToAction("BranchAdd");
            // return RedirectToAction("BranchList");
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
