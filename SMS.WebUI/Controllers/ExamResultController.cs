using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.BLL.SMSService;
using SMS.DTO;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class ExamResultController : Controller
    {
        private readonly IExamResultService examResultService;
        private readonly IExamService examService;
        private readonly IExamTypeService examTypeService;
        private readonly ISubjectService subjectService;
        private readonly IStudentService studentService;
        private readonly IUserService userService;
        private readonly IInstructorService instructorService;

        public ExamResultController(IExamResultService _examResultService, IExamService _examService, IExamTypeService _examTypeService, IStudentService _studentService, IUserService _userService, ISubjectService _subjectService, IInstructorService _instructorService)
        {
            examResultService = _examResultService;
            examService = _examService;
            examTypeService = _examTypeService;
            studentService = _studentService;
            userService = _userService;
            subjectService = _subjectService;
            instructorService = _instructorService;
        }
        public IActionResult ExamResultList(int examId)
        {
            List<ExamResultDTO> examResults = examResultService.GetExamResultsOfExam(examId);
            ViewBag.Exam = examService.GetExam(examId).ExamName;
            return View(examResults);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        [HttpGet]
        public IActionResult ExamResultAdd(int studentId, int? examId, int? subjectId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);

            if (examId != null)
            {
                model.ExamDTO = examService.GetExam((int)examId);
                model.ExamResultDTO = new ExamResultDTO() { StudentId = studentId, ExamId = (int)examId };
                ViewBag.SubjectName = model.ExamDTO.Subject.SubjectName;
            }
            else if (subjectId != null)
            {
                model.ExamDTOs = examService.GetExamBySubject((int)subjectId);
                model.ExamResultDTO = new ExamResultDTO() { StudentId = studentId };
                ViewBag.SubjectName = subjectService.GetSubject((int)subjectId).SubjectName;
            }
            return PartialView(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        [HttpPost]
        public IActionResult ExamResultAdd(StudentDetailsViewModel model)
        {
            var user = userService.GetUserByUsername(User.Identity.Name);
            model.ExamResultDTO.CreatedBy = instructorService.GetInstructorByUsername(user.UserName).Id;
            examResultService.NewExamResult(model.ExamResultDTO);

            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult ExamResultDelete(int examResultId)
        {
            examResultService.DeleteExamResult(examResultId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        [HttpGet]
        public IActionResult ExamResultUpdate(int examResultId)
        {
            ExamResultDTO examResult = examResultService.GetExamResult(examResultId);
            return PartialView(examResult);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        [HttpPost]
        public IActionResult ExamResultUpdate(ExamResultDTO examResult)
        {
            examResultService.UpdateExamResult(examResult);
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
