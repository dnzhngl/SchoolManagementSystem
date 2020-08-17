using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;

namespace SMS.WebUI.Controllers
{
    public class SemesterInformationController : Controller
    {
        private readonly ISemesterInformationService semesterInformationService;
        public SemesterInformationController(ISemesterInformationService _semesterInformationService)
        {
            semesterInformationService = _semesterInformationService;
        }
        public IActionResult SemesterList()
        {
            return View(semesterInformationService.GetAll());
        }
        public IActionResult SemesterAdd()
        {
            var model = new SemesterInformationDTO();
            model.SemesterBeginning = DateTime.Today;
            model.SemesterEnd = DateTime.Today;
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult SemesterAdd(SemesterInformationDTO semester)
        {
            semesterInformationService.NewSemester(semester);
            return RedirectToAction("SemesterList");
        }
        public IActionResult SemesterDelete(int id)
        {
            semesterInformationService.DeleteSemester(id);
            return RedirectToAction("SemesterList");
        }
        public IActionResult SemesterUpdate(int id)
        {
            SemesterInformationDTO selectedSemester = semesterInformationService.GetSemesterInfo(id);
            return PartialView(selectedSemester);
        }
        [HttpPost]
        public IActionResult SemesterUpdate(SemesterInformationDTO semester)
        {
            semesterInformationService.UpdateSemester(semester);
            return RedirectToAction("SemesterList");
        }
    }
}
