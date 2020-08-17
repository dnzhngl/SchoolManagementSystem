using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.BLL.SMSService;
using SMS.DTO;
using SMS.Model;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;
        private readonly IParentService parentService;
        private readonly ISectionService sectionService;
        private readonly IGradeService gradeService;

        public StudentController(IStudentService _studentService, IParentService _parentService, ISectionService _sectionService, IGradeService _gradeService)
        {
            studentService = _studentService;
            parentService = _parentService;
            sectionService = _sectionService;
            gradeService = _gradeService;
        }

        public IActionResult RegistrationList()
        {
            return View(studentService.GetAll());
        }

        //public IActionResult RegistrationAdd()
        //{
        //    StudentParentViewModel model = new StudentParentViewModel();
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult RegistrationAdd(StudentParentViewModel student)
        //{

        //    StudentDTO newStudent = student.StudentDTO;
        //    newStudent.ParentId = student.ParentDTO.Id;

        //    studentService.NewStudent(newStudent);

        //    return RedirectToAction("RegistrationList");
        //}

        public IActionResult RegistrationDelete(int id)
        {
            studentService.DeleteStudent(id);
            return RedirectToAction("RegistrationList");
        }


        public IActionResult StudentAdd(int parentId)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.ParentDTO = parentService.GetParent(parentId);
           // model.SectionDTOs = sectionService.GetAll();
 
            return View(model);
        }
        [HttpPost]
        public IActionResult StudentAdd(StudentParentViewModel student)
        {
            StudentDTO newStudent = student.StudentDTO;
            newStudent.ParentId = student.ParentDTO.Id;

            studentService.NewStudent(newStudent);
            return RedirectToAction("RegistrationList");
        }

        public IActionResult StudentList(int? id)
        {
            List<StudentDTO> studentList;
            if (id != null)
            {
                studentList = studentService.GetStudentBySection((int)id);
            }
            else
            {
                studentList = studentService.GetAll();
            }
            return View(studentList);
        }

        public IActionResult RegisteredStudentDetails(int id)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.StudentDTO = studentService.GetStudent(id);
            model.ParentDTO = parentService.GetParent((int)model.StudentDTO.ParentId);
            return View(model);
        }

        public IActionResult StudentUpdate(int id)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            StudentDTO selectedStudent = studentService.GetStudent(id);
            selectedStudent.ParentDTO = parentService.GetParent((int)selectedStudent.ParentId);

            model.StudentDTO = selectedStudent;
            model.ParentDTO = selectedStudent.ParentDTO;

            return View(model);
        }
        [HttpPost]
        public IActionResult StudentUpdate(StudentParentViewModel student)
        {
            StudentDTO selectedStudent = student.StudentDTO;
            selectedStudent.ParentId = student.ParentDTO.Id;
            selectedStudent.ParentDTO = student.ParentDTO;
            studentService.UpdateStudent(selectedStudent);
            return RedirectToAction("StudentDetails");
        }

        public IActionResult AssignSection(int id)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            StudentDTO selectedStudent = studentService.GetStudent(id);
            model.StudentDTO = selectedStudent;
            model.GradeDTOs = gradeService.GetAll();
            model.SectionDTOs = sectionService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult AssignSection(StudentDetailsViewModel studentDetail)
        {
            StudentDTO student = studentDetail.StudentDTO;
            studentService.UpdateStudent(student);

            return RedirectToAction("RegistrationList");
        }

    }
}
