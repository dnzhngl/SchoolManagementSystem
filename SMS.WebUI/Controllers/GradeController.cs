
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;

namespace SMS.WebUI.Controllers
{
    public class GradeController : Controller
    {
        private readonly IGradeService gradeService;
        private readonly ISectionService sectionService;
        public GradeController(IGradeService _gradeService, ISectionService _sectionService)
        {
            gradeService = _gradeService;
            sectionService = _sectionService;
        }

        public IActionResult GradeList()
        {
            return View(gradeService.GetAll());
        }

        public IActionResult GradeAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult GradeAdd(GradeDTO grade)
        {
            gradeService.NewGrade(grade);
            return RedirectToAction("GradeList");
        }
        public IActionResult GradeDelete(int id)
        {
            gradeService.DeleteGrade(id);
            return RedirectToAction("GradeList");
        }
        public IActionResult GradeUpdate(int id)
        {
            GradeDTO selectedGrade = gradeService.GetGrade(id);
            return PartialView(selectedGrade);
        }
        [HttpPost]
        public IActionResult GradeUpdate(GradeDTO grade)
        {
            gradeService.UpdateGrade(grade);
            return RedirectToAction("GradeList");
        }
    }
}
