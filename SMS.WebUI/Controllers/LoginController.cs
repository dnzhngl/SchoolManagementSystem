using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;

namespace SMS.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IRoleService roleService;
        public LoginController(IRoleService _roleService)
        {
            roleService = _roleService;
        }
        public IActionResult UserLogin()
        {
            return View();
        }
    }
}
