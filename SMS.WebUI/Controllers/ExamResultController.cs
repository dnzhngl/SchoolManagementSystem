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
    public class ExamResultController : Controller
    {
        private readonly IExamResultService examResultService;
        private readonly IExamService examService;
        private readonly IExamTypeService examTypeService;
        private readonly IStudentService studentService;

        public ExamResultController(IExamResultService _examResultService, IExamService _examService, IExamTypeService _examTypeService, IStudentService _studentService)
        {
            examResultService = _examResultService;
            examService = _examService;
            examTypeService = _examTypeService;
            studentService = _studentService;
        }
        [HttpGet]
        public IActionResult ExamResultAdd(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ExamResultAdd(StudentDetailsViewModel model)
        {
            examResultService.NewExamResult(model.ExamResultDTO);
            return RedirectToAction();
        }
        public IActionResult StudentExamResults(int studentId, string? studentUserName) 
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            if (studentUserName != null)
            {
                studentId = studentService.GetStudentByUsername(studentUserName).Id;
            }
            model.StudentDTO = studentService.GetStudent(studentId);
            model.ExamResultDTOs = examResultService.GetExamResultsOfStudent(studentId);
            return View(model);
        }
        [HttpGet]
        public IActionResult ExamResultUpdate(int examResultId)
        {
            ExamResultDTO model = new ExamResultDTO();
            model = examResultService.GetExamResult(examResultId);

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ExamResultUpdate(ExamResultDTO examResult)
        {
            examResultService.UpdateExamResult(examResult);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
