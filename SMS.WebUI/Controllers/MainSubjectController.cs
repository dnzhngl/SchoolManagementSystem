using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;

namespace SMS.WebUI.Controllers
{
    public class MainSubjectController : Controller
    {
        private readonly IMainSubjectService mainSubjectService;
        public MainSubjectController(IMainSubjectService _mainSubjectService)
        {
            mainSubjectService = _mainSubjectService;
        }
        public IActionResult MainSubjectList()
        {
            return View(mainSubjectService.GetAll());
        }
        public IActionResult MainSubjectAdd()
        {
            return PartialView(); 
        }
        [HttpPost]
        public IActionResult MainSubjectAdd(MainSubjectDTO mainSubject)
        {
            mainSubjectService.NewMainSubject(mainSubject);
            return RedirectToAction("MainSubjectList");
        }
        public IActionResult MainSubjectDelete(int id)
        {
            mainSubjectService.DeleteMainSubject(id);
            return RedirectToAction("MainSubjectList");
        }
        public IActionResult MainSubjectUpdate(int id)
        {
            MainSubjectDTO selectedMainSubject = mainSubjectService.GetMainSubject(id);
            return PartialView(selectedMainSubject);
        }
        [HttpPost]
        public IActionResult MainSubjectUpdate(MainSubjectDTO mainSubject)
        {
            mainSubjectService.UpdateMainSubject(mainSubject);
            return RedirectToAction("MainSubjectList");
        }


    }
}
