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
        private readonly IUserService userService;


        public StudentController(IStudentService _studentService, IParentService _parentService, ISectionService _sectionService, IGradeService _gradeService, IUserService _userService)
        {
            studentService = _studentService;
            parentService = _parentService;
            sectionService = _sectionService;
            gradeService = _gradeService;
            userService = _userService;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegistrationList()
        {
            var registrationList = studentService.GetAll().Where(z => z.SectionId == null);
            return View(registrationList);
        }

        public IActionResult RegistrationDelete(int id)
        {
            var student = studentService.GetStudent(id);
            int userId = (int)student.UserId;
            userService.DeleteUser(userId);

            var parent = parentService.GetParent(student.ParentId);
            userService.DeleteUser((int)parent.UserId);
            parentService.DeleteParent(student.ParentId);

            studentService.DeleteStudent(id);

            return RedirectToAction("RegistrationList");
        }
        public IActionResult RegisteredStudentDetails(int id)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.StudentDTO = studentService.GetStudent(id);
            if (model.StudentDTO.SectionId != null)
            {
                model.StudentDTO.SectionDTO = sectionService.GetSection((int)model.StudentDTO.SectionId);
            }
            model.ParentDTO = parentService.GetParent((int)model.StudentDTO.ParentId);
            return View(model);
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
            userService.NewUser(newStudent.IdentityNumber, "Öğrenci");

            UserDTO newUser = userService.GetUserByUsername(newStudent.IdentityNumber);
            newStudent.UserId = newUser.Id;

            studentService.NewStudent(newStudent);
            return RedirectToAction("RegistrationList");
        }

        public IActionResult StudentUpdate(int id)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            StudentDTO selectedStudent = studentService.GetStudent(id);
            selectedStudent.ParentDTO = parentService.GetParent((int)selectedStudent.ParentId);

            model.StudentDTO = selectedStudent;
            model.ParentDTO = selectedStudent.ParentDTO;

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult StudentUpdate(StudentParentViewModel student)
        {
            StudentDTO selectedStudent = student.StudentDTO;
            selectedStudent.ParentId = student.ParentDTO.Id;
            selectedStudent.ParentDTO = parentService.GetParent((int)selectedStudent.ParentId);
            studentService.UpdateStudent(selectedStudent);
            return RedirectToAction("RegisteredStudentDetails", new { id = selectedStudent.Id });
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
            StudentDTO student = studentService.GetStudent(studentDetail.StudentDTO.Id);
            student.SectionId = studentDetail.StudentDTO.SectionId;
            studentService.UpdateStudent(student);

            return RedirectToAction("RegistrationList");
        }
        public IActionResult StudentList(int? id, string? sectionName, string? parentUsername)
        {
            // id for Section
            StudentDetailsViewModel model = new StudentDetailsViewModel();

            if (id != null)
            {
                model.StudentDTOs = studentService.GetStudentBySection((int)id);                
            }
            else if (sectionName != null)
            {
                var section = sectionService.GetSectionByName(sectionName);
                model.StudentDTOs = studentService.GetStudentBySection(section.Id);
            }
            else if (parentUsername != null)
            {
                var userId = userService.GetUserByUsername(parentUsername).Id;
                var parent = parentService.GetParentByUserId(userId);
                model.StudentDTOs = studentService.GetStudentByParent(parent.Id);
            }
            else
            {
                model.StudentDTOs = studentService.GetAllStudents();
            }
            model.GradeDTOs = gradeService.GetAll();
            // model.SectionDTOs = sectionService.GetAll(); Orjinalde olan.

            model.SectionDTOs = sectionService.GetSectionsWithGrade();

            return View(model);
        }
    }
}
