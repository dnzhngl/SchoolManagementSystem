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
    public class ExamController : Controller
    {
        private readonly IExamService examService;
        private readonly IExamResultService examResultService;
        private readonly IExamTypeService examTypeService;
        private readonly ISubjectService subjectService;
        private readonly IMainSubjectService mainSubjectService;
        private readonly IStudentService studentService;

        public ExamController(IExamService _examService, IExamTypeService _examTypeService, ISubjectService _subjectService, IMainSubjectService _mainSubjectService, IStudentService _studentService, IExamResultService _examResultService)
        {
            examService = _examService;
            examTypeService = _examTypeService;
            subjectService = _subjectService;
            mainSubjectService = _mainSubjectService;
            studentService = _studentService;
            examResultService = _examResultService;
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Öğrenci, Veli")]
        public IActionResult ExamList(int? id, int? studentId, string subjectName = "", string studentUsername = "")
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            if (id != null)
            {
                model.ExamDTOs = examService.GetExamBySubject((int)id);
                model.SubjectDTO = subjectService.GetSubject((int)id);
                model.SubjectDTO.Exams = model.ExamDTOs; //
            }
            else if (subjectName != "")
            {
                model.SubjectDTO = subjectService.GetSubject(subjectName);
                model.ExamDTOs = examService.GetExamBySubject((int)model.SubjectDTO.Id);
            }
            else if (studentUsername != "")
            {
                var student = studentService.GetStudentByUsername(studentUsername);
               // model.ExamDTOs = examService.StudentsExamList(student.Id);
                model.ExamDTOs = examService.GetExamsByStudent((int)student.Id);
            }
            else if (studentId != null)
            {
                model.ExamDTOs = examService.GetExamsByStudent((int)studentId);
            }
            else
            {
                model.ExamDTOs = examService.GetAll();
            }
            model.SubjectDTOs = subjectService.GetAll();
            model.ExamTypeDTOs = examTypeService.GetAll();
            return View(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult ExamAdd(int? subjectId, string subjectName = "")
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            if (subjectId != null)
            {
                model.SubjectDTO = subjectService.GetSubject((int)subjectId);
            }
            else if (subjectName != "")
            {
                model.SubjectDTO = subjectService.GetSubject(subjectName);
            }
            else
            {
                model.SubjectDTOs = subjectService.GetAll();
            }
            model.ExamTypeDTOs = examTypeService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult ExamAdd(SubjectDetailViewModel exam)
        {
            ExamDTO newExam = exam.ExamDTO;
            examService.NewExam(newExam);

            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult ExamDelete(int id)
        {
            examService.DeleteExam(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult ExamUpdate(int id)
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            model.ExamDTO = examService.GetExam(id);
            model.ExamTypeDTOs = examTypeService.GetAll();

            return PartialView(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        [HttpPost]
        public IActionResult ExamUpdate(SubjectDetailViewModel exam)
        {
            ExamDTO selectedExam = exam.ExamDTO;
            examService.UpdateExam(selectedExam);

            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
