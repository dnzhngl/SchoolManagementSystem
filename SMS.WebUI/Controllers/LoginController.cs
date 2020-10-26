using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.WebUI.Core;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IAdminService adminService;
        public LoginController(IRoleService _roleService, IUserService _userService, IAdminService _adminService)
        {
            userService = _userService;
            roleService = _roleService;
            adminService = _adminService;
        }
        [HttpGet]
        public IActionResult UserLogin()

        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(LoginViewModel userModel)
        {
            var user = userService.FindWithUsernameAndMail(userModel.UserName, userModel.Password);
            if (user != null)
            {
                user.Role = roleService.GetRole((int)user.RoleId);
                var userClaims = new List<Claim>()
                {
                       new Claim("UserDTO",SMSConvert.SMSJsonSerialize(user)),
                       new Claim(ClaimTypes.Name, user.UserName),
                       new Claim(ClaimTypes.Role, user.Role.RoleName),
                       new Claim(ClaimTypes.Email, user.Email)
            };
                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity, new ClaimsIdentity() });
                HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index", "User", user);
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("UserLogin");
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AdminRegistration()
        {
            var admin = new AdminDTO();
            admin.User = new UserDTO();

            return View(admin);
        }
        [HttpPost]
        public ActionResult AdminRegistration(AdminDTO admin)
        {
            admin.User = userService.GenerateUserAccount(admin.FirstName, admin.LastName, admin.User.Password, "Admin");
            admin.UserId = admin.User.Id;

            admin.User = null;

            adminService.NewAdmin(admin);

            return RedirectToAction("UserLogin");
        }
    }
}
