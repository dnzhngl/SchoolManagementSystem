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

        public IActionResult ExamList(int? id, string? subjectName, string? studentUsername, int? studentId)
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            if (id != null)
            {
                model.ExamDTOs = examService.GetExamBySubject((int)id);
                model.SubjectDTO = subjectService.GetSubject((int)id);
                model.SubjectDTO.Exams = model.ExamDTOs; //
            }
            else if (subjectName != null)
            {
                model.SubjectDTO = subjectService.GetSubject(subjectName);
                model.ExamDTOs = examService.GetExamBySubject((int)model.SubjectDTO.Id);
            }
            else if (studentUsername != null)
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


        public IActionResult ExamAdd(int? subjectId, string? subjectName)
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();
            if (subjectId != null)
            {
                model.SubjectDTO = subjectService.GetSubject((int)subjectId);
            }
            else if (subjectName != null)
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
        public IActionResult ExamAdd(SubjectDetailViewModel exam)
        {
            ExamDTO newExam = exam.ExamDTO;
           // newExam.SubjectId = exam.SubjectDTO.Id; //
            examService.NewExam(newExam);

            return Redirect(Request.Headers["Referer"].ToString());

            //int subjectId = newExam.SubjectId;
           // return RedirectToAction("SubjectDetails","Subject", new { subjectId = subjectId });
            //return RedirectToAction("SubjectDetails", "Subject", new { id = exam.SubjectDTO.Id }); //ExamListBySubject
        }
        public IActionResult ExamDelete(int id)
        {
            var subjectId = examService.GetExam(id).SubjectId;

            examService.DeleteExam(id);

            //return RedirectToAction("SubjectDetails", "Subject", new { id = subjectId });
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult ExamUpdate(int id)
        {
            SubjectDetailViewModel model = new SubjectDetailViewModel();

            model.ExamDTO = examService.GetExam(id);
            model.ExamTypeDTOs = examTypeService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ExamUpdate(SubjectDetailViewModel exam)
        {
            ExamDTO selectedExam = exam.ExamDTO;
            //selectedExam.ExamType = examTypeService.GetExamType(exam.ExamDTO.ExamTypeId);
            //selectedExam.Subject = subjectService.GetSubject(exam.ExamDTO.SubjectId);
            examService.UpdateExam(selectedExam);

            return Redirect(Request.Headers["Referer"].ToString());
            //return RedirectToAction("SubjectDetails", "Subject", new { subjectId = exam.ExamDTO.SubjectId });
        }

        public IActionResult StudentExamsList(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.ExamDTOs = examService.GetExamsOfStudent(studentId);
            model.ExamResultDTOs = examResultService.GetExamResultsOfStudent(studentId);
            model.StudentDTO = studentService.GetStudent(studentId);
            return View(model);
        }
    }
}
