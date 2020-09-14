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
    public class ExamController : Controller
    {
        private readonly IExamService examService;
        private readonly IExamTypeService examTypeService;
        private readonly ISubjectService subjectService;
        private readonly IMainSubjectService mainSubjectService;

        public ExamController(IExamService _examService, IExamTypeService _examTypeService, ISubjectService _subjectService, IMainSubjectService _mainSubjectService)
        {
            examService = _examService;
            examTypeService = _examTypeService;
            subjectService = _subjectService;
            mainSubjectService = _mainSubjectService;
        }

        public IActionResult ExamList(int? id, string? subjectName)
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            if (id != null)
            {
                model.ExamDTOs = examService.GetExamBySubject((int)id);
                model.SubjectDTO = subjectService.GetSubject((int)id);
                model.SubjectDTO.ExamDTOs = model.ExamDTOs; //
            }
            else if (subjectName != null)
            {
                model.SubjectDTO = subjectService.GetSubjectByName(subjectName);
                model.ExamDTOs = examService.GetExamBySubject((int)model.SubjectDTO.Id);
            }
            else
            {
                model.ExamDTOs = examService.GetAll();
            }
            model.SubjectDTOs = subjectService.GetAll();
            model.ExamTypeDTOs = examTypeService.GetAll();
            return View(model);
        }


        public IActionResult ExamAdd(int? id, string? subjectName)
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            if (id != null)
            {
                model.SubjectDTO = subjectService.GetSubject((int)id);
                //var mainSubjectId = model.SubjectDTO.MainSubjectId;
                //model.MainSubjectDTO = mainSubjectService.GetMainSubject(mainSubjectId);
            }
            else if (subjectName != null)
            {
                model.SubjectDTO = subjectService.GetSubjectByName(subjectName);
            }
            else
            {
                model.SubjectDTOs = subjectService.GetAll();
            }
            model.ExamTypeDTOs = examTypeService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ExamAdd(SubjectDetailViewModel exam)
        {
            ExamDTO newExam = exam.ExamDTO;
            newExam.SubjectId = exam.SubjectDTO.Id;
            examService.NewExam(newExam);

            int subjectId = newExam.SubjectId;

            return RedirectToAction("SubjectDetails","Subject", new { id = subjectId });
            //return RedirectToAction("SubjectDetails", "Subject", new { id = exam.SubjectDTO.Id }); //ExamListBySubject
        }
        public IActionResult ExamDelete(int id)
        {
            var subjectId = examService.GetExam(id).SubjectId;

            examService.DeleteExam(id);

            return RedirectToAction("SubjectDetails", "Subject", new { id = subjectId });
        }
        public IActionResult ExamUpdate(int id)
        {
            ExamDTO selectedExam = examService.GetExam(id);
            SubjectDetailViewModel model = new SubjectDetailViewModel();

            model.ExamDTO = selectedExam;
            model.ExamTypeDTO = examTypeService.GetExamType(selectedExam.ExamTypeId);
            model.ExamTypeDTOs = examTypeService.GetAll();
            model.ExamDTO.SubjectDTO = subjectService.GetSubject(model.ExamDTO.SubjectId);

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ExamUpdate(SubjectDetailViewModel exam)
        {
            ExamDTO selectedExam = exam.ExamDTO;
            selectedExam.ExamTypeDTO = examTypeService.GetExamType(exam.ExamDTO.ExamTypeId);
            selectedExam.SubjectDTO = subjectService.GetSubject(exam.ExamDTO.SubjectId);
            examService.UpdateExam(selectedExam);

            return RedirectToAction("SubjectDetails", "Subject", new { id = exam.ExamDTO.SubjectId });
        }
    }
}
