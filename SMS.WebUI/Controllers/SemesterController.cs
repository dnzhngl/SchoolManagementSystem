using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;

namespace SMS.WebUI.Controllers
{
    public class SemesterController : Controller
    {
        private readonly ISemesterService semesterService;
        public SemesterController(ISemesterService _semesterInformationService)
        {
            semesterService = _semesterInformationService;
        }
        public IActionResult SemesterList()
        {
            return View(semesterService.GetAll());
        }
        public IActionResult SemesterAdd()
        {
            var model = new SemesterDTO();
            model.SemesterBeginning = DateTime.Today;
            model.SemesterEnd = DateTime.Today;
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult SemesterAdd(SemesterDTO semester)
        {
            semesterService.NewSemester(semester);
            return RedirectToAction("SemesterList");
        }
        public IActionResult SemesterDelete(int id)
        {
            semesterService.DeleteSemester(id);
            return RedirectToAction("SemesterList");
        }
        public IActionResult SemesterUpdate(int id)
        {
            SemesterDTO selectedSemester = semesterService.GetSemesterInfo(id);
            return PartialView(selectedSemester);
        }
        [HttpPost]
        public IActionResult SemesterUpdate(SemesterDTO semester)
        {
            semesterService.UpdateSemester(semester);
            return RedirectToAction("SemesterList");
        }
    }
}
