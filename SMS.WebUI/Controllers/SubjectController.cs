using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService subjectService;
        private readonly IMainSubjectService mainSubjectService;
        private readonly IExamService examService;
        private readonly IExamTypeService examTypeService;
        public SubjectController(ISubjectService _subjectService, IMainSubjectService _mainSubjectService, IExamService _examService, IExamTypeService _examTypeService)
        {
            subjectService = _subjectService;
            mainSubjectService = _mainSubjectService;
            examService = _examService;
            examTypeService = _examTypeService;
        }

        public IActionResult SubjectList(int? mainSubjectId)
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            if (mainSubjectId != null)
            {
                model.SubjectDTOs = subjectService.GetSubjectByMainSubject((int)mainSubjectId);
            }
            else
            {
                model.SubjectDTOs = subjectService.GetAll(); 
            }
            model.MainSubjectDTOs = mainSubjectService.GetAll();
            return View(model);
        }
        public IActionResult SubjectAdd()
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            model.MainSubjectDTOs = mainSubjectService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult SubjectAdd(SubjectDetailViewModel subject)
        {
            SubjectDTO newSubject = subject.SubjectDTO;
            subjectService.NewSubject(newSubject);
            return RedirectToAction("SubjectList");
        }
        public IActionResult SubjectDelete(int id)
        {
            subjectService.DeleteSubject(id);
            return RedirectToAction("SubjectList");
        }
        public IActionResult SubjectUpdate(int id)
        {
            SubjectDTO selectedSubject = subjectService.GetSubject(id);
            SubjectDetailViewModel model = new SubjectDetailViewModel();

            model.SubjectDTO = selectedSubject;
            model.MainSubjectDTO = mainSubjectService.GetMainSubject(selectedSubject.MainSubjectId);
            model.MainSubjectDTOs = mainSubjectService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult SubjectUpdate(SubjectDetailViewModel subject)
        {
            SubjectDTO selectedSubject = subject.SubjectDTO;
            subjectService.UpdateSubject(selectedSubject);
            return RedirectToAction("SubjectList");
        }

        public IActionResult SubjectDetails(int id)
        {
            SubjectDTO selectedSubject = subjectService.GetSubject(id);
            SubjectDetailViewModel model = new SubjectDetailViewModel();

            model.SubjectDTO = selectedSubject;
            model.MainSubjectDTO = mainSubjectService.GetMainSubject(selectedSubject.MainSubjectId);
            model.ExamDTOs = examService.GetExamBySubject(id);
            model.ExamTypeDTOs = examTypeService.GetAll();

            return View(model);
        }
    }
}
