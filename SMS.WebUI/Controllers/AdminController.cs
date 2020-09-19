using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;

namespace SMS.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Yönetici")]
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        public AdminController(IAdminService _adminService, IUserService _userService, IRoleService _roleService)
        {
            adminService = _adminService;
            userService = _userService;
            roleService = _roleService;
        }

        public IActionResult Index(int id)
        {
            AdminDTO admin = adminService.GetAdmin(id);
            return View(admin);
        }
        public IActionResult AdminList()
        {
            return View(adminService.GetAll());
        }
        public IActionResult AdminAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult AdminAdd(AdminDTO admin)
        {
            UserDTO newUser = userService.GenerateUserAccount(admin.FirstName, admin.LastName, admin.IdentityNumber,  "Admin");            
            admin.UserId = newUser.Id;

            adminService.NewAdmin(admin);
            return RedirectToAction("AdminList");
        }
        public IActionResult AdminDelete(int id)
        {
            int userId = (int)adminService.GetAdmin(id).UserId;
            userService.DeleteUser(userId);

            adminService.DeleteAdmin(id);
            return RedirectToAction("AdminList");
        }
        public IActionResult AdminUpdate(int id)
        {
            AdminDTO selectedAdmin = adminService.GetAdmin(id);
            return PartialView(selectedAdmin);
        }
        [HttpPost]
        public IActionResult AdminUpdate(AdminDTO admin)
        {
            adminService.UpdateAdmin(admin);
            return RedirectToAction("AdminList");
        }
        public IActionResult AdminDetails(int id)
        {
            AdminDTO selectedAdmin = adminService.GetAdmin(id);
            return View(selectedAdmin);
        }
    }
}

