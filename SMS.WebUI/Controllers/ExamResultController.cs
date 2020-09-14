using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SMS.WebUI.Controllers
{
    public class ExamResultController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
