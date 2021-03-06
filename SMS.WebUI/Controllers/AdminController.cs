﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IPostService postService;
        public AdminController(IAdminService _adminService, IUserService _userService, IRoleService _roleService, IPostService _postService)
        {
            adminService = _adminService;
            userService = _userService;
            roleService = _roleService;
            postService = _postService;
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult Index()
        {
            MainPageViewModel model = new MainPageViewModel();
            model.PostDTOs = postService.GetAll();
            model.UserDTO = userService.GetUserByUsername(this.User.Identity.Name);
            return View(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli, Öğrenci")]
        public IActionResult AdminList()
        {
            return View(adminService.GetAll());
        }

        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult AdminAdd()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin, Yönetici")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminAdd(AdminDTO admin)
        {
            if (ModelState.IsValid)
            {
                UserDTO newUser = userService.GenerateUserAccount(admin.FirstName, admin.LastName, admin.IdentityNumber, "Admin");
                admin.UserId = newUser.Id;

                adminService.NewAdmin(admin);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return View(admin);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult AdminDelete(int id)
        {
            int userId = (int)adminService.GetAdmin(id).UserId;
            userService.DeleteUser(userId);

            adminService.DeleteAdmin(id);
            return RedirectToAction("AdminList");
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult AdminUpdate(int id)
        {
            AdminDTO selectedAdmin = adminService.GetAdmin(id);
            return PartialView(selectedAdmin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult AdminUpdate(AdminDTO admin)
        {
            if (ModelState.IsValid)
            {
                adminService.UpdateAdmin(admin);
                return RedirectToAction("AdminList");
            }
            return View(admin);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult AdminDetails(int id)
        {
            AdminDTO selectedAdmin = adminService.GetAdmin(id);
            return View(selectedAdmin);
        }
    }
}

