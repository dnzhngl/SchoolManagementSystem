﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public SectionController(ISectionService _sectionService, IGradeService _gradeService)
        {
            sectionService = _sectionService;
            gradeService = _gradeService;
        }
        public IActionResult SectionList(int? id)
        {
            SectionGradeViewModel model = new SectionGradeViewModel();
            if (id != null)
            {
                model.SectionDTOs = sectionService.GetSectionByGrade((int)id);
            }
            else
            {
                model.SectionDTOs = sectionService.GetAll();
            }
            model.GradeDTOs = gradeService.GetAll();
            return View(model);
        }

        public IActionResult SectionAdd()
        {
            SectionGradeViewModel model = new SectionGradeViewModel();
            model.GradeDTOs = gradeService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult SectionAdd(SectionGradeViewModel section)
        {
            SectionDTO newSection = section.SectionDTO;
            sectionService.NewSection(newSection);

            return RedirectToAction("SectionList");
        }
        public IActionResult SectionDelete(int id)
        {
            sectionService.DeleteSection(id);
            return RedirectToAction("SectionList");
        }

        public IActionResult SectionUpdate(int id)
        {
            SectionDTO selectedSection = sectionService.GetSection(id);
            SectionGradeViewModel model = new SectionGradeViewModel();

            model.SectionDTO = selectedSection;
            model.GradeDTO = gradeService.GetGrade(selectedSection.GradeId);
            model.GradeDTOs = gradeService.GetAll();

            return View(model);
        }
        [HttpPost]
        public IActionResult SectionUpdate(SectionGradeViewModel section)
        {
            SectionDTO selectedSection = section.SectionDTO;
            selectedSection.GradeId = section.GradeDTO.Id;
            sectionService.UpdateSection(selectedSection);

            return RedirectToAction("SectionList");
        }

    }
}