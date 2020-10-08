using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;

namespace SMS.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        public UserController(IUserService _userService, IRoleService _roleService)
        {
            userService = _userService;
            roleService = _roleService;
        }
        public IActionResult Index(UserDTO userDTO)
        {
            var userRole = roleService.GetRole((int)userDTO.RoleId).RoleName;

            if (userRole.Contains("Admin"))
            {
                return RedirectToAction("Index","Admin", new { id = userDTO.Id });
            }
            else if (userRole.Contains("Öğretmen"))
            {
                return RedirectToAction("Index","Instructor", new { id = userDTO.Id});
            }
            else if (userRole.Contains("Veli"))
            {
                return RedirectToAction("Index", "Parent", new { userId = userDTO.Id });
            }
            else if (userRole.Contains("Öğrenci"))
            {
                return RedirectToAction("Index","Student", new { id = userDTO.Id });
            }
            return View();
        }
        public IActionResult UserDelete(int userId)
        {
            userService.DeleteUser(userId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult UserSettings(string username)
        {
            UserDTO user = userService.GetUserByUsername(username);
            return View(user);
        }
        [HttpGet]
        public IActionResult ChangePassword(string username)
        {
            UserDTO user = userService.GetUserByUsername(username);
            return PartialView(user);
        }
        [HttpPost]
        public IActionResult ChangePassword(UserDTO user, string newPassword)
        {
            user.Password = newPassword;
            userService.UpdateUser(user);

            return RedirectToAction("UserSettings", new { username = user.UserName });
        }

    }
}
