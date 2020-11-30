using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionService sectionService;
        private readonly IGradeService gradeService;
        private readonly IStudentService studentService;
        private readonly IInstructorService instructorService;
        public SectionController(ISectionService _sectionService, IGradeService _gradeService, IStudentService _studentService, IInstructorService _instructorService)
        {
            sectionService = _sectionService;
            gradeService = _gradeService;
            studentService = _studentService;
            instructorService = _instructorService;
        }
        public IActionResult SectionList(int? gradeId)
        {
            SectionGradeViewModel model = new SectionGradeViewModel();
            //List<SectionDTO> sections = new List<SectionDTO>();
            if (gradeId != null)
            {
                model.SectionDTOs = sectionService.GetSectionByGrade((int)gradeId);
            }
            else
            {
                model.SectionDTOs = sectionService.GetAll();
            }
            model.InstructorDTOs = instructorService.GetInstructorNameWithBranch();
            return View(model);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult SectionAdd()
        {
            SectionGradeViewModel model = new SectionGradeViewModel();
            model.GradeDTOs = gradeService.GetAll();
            model.InstructorDTOs = instructorService.GetInstructorNameWithBranch();
            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult SectionAdd(SectionGradeViewModel section)
        {
            SectionDTO newSection = section.SectionDTO;
            sectionService.NewSection(newSection);

            return RedirectToAction("SectionList");
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult SectionDelete(int id)
        {
            sectionService.DeleteSection(id);
            return RedirectToAction("SectionList");
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult SectionUpdate(int id)
        {
            SectionGradeViewModel model = new SectionGradeViewModel();

            model.SectionDTO = sectionService.GetSection(id);
            model.SectionDTO.Grade = gradeService.GetGrade(model.SectionDTO.GradeId);
            model.GradeDTOs = gradeService.GetAll();
            model.InstructorDTOs = instructorService.GetInstructorNameWithBranch();

            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult SectionUpdate(SectionGradeViewModel model)
        {
            sectionService.UpdateSection(model.SectionDTO);
            return RedirectToAction("SectionList");
        }

    }
}
