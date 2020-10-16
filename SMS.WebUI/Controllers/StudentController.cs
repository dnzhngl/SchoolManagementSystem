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
        private readonly ICertificateTypeService certificateTypeService;
        private readonly ICertificateService certificateService;
        private readonly IPostService postService;
        private readonly IExamResultService examResultService;

        public StudentController(IStudentService _studentService, IParentService _parentService, ISectionService _sectionService, IGradeService _gradeService, IUserService _userService, ICertificateTypeService _certificateTypeService, IPostService _postService, IExamResultService _examResultService, ICertificateService _certificateService)
        {
            studentService = _studentService;
            parentService = _parentService;
            sectionService = _sectionService;
            gradeService = _gradeService;
            userService = _userService;
            certificateTypeService = _certificateTypeService;
            certificateService = _certificateService;
            postService = _postService;
            examResultService = _examResultService;
        }
        public IActionResult Index()
        {
            MainPageViewModel model = new MainPageViewModel();
            model.PostDTOs = postService.GetAll();
            model.UserDTO = userService.GetUserByUsername(this.User.Identity.Name);
            return View(model);
        }
        public IActionResult RegistrationList()
        {
            var registrationList = studentService.GetAll().Where(z => z.SectionId == null);
            return View(registrationList);
        }

        public IActionResult StudentDelete(int id)
        {
            var student = studentService.GetStudent(id);

            int userId = (int)student.UserId;
            userService.DeleteUser(userId);

            int parentId = student.ParentId;
            //var parent = parentService.GetParent(student.ParentId);
            //userService.DeleteUser((int)parent.UserId);
            //parentService.DeleteParent(student.ParentId);

            studentService.DeleteStudent(id);

            //return RedirectToAction("RegistrationList");
            // return Redirect(Request.Headers["Referer"].ToString());

            return RedirectToAction("ParentDelete", "Parent", new { id = parentId });
        }

        public IActionResult StudentList(int? sectionId, string? sectionName, string? parentUsername, string? certificateTypeName, string? studentStatus, int? gradeId)
        {
            // id for Section
            StudentDetailsViewModel model = new StudentDetailsViewModel();

            if (sectionId != null)
            {
                model.StudentDTOs = studentService.GetStudentBySection((int)sectionId);
                var section_Name = sectionService.GetSection((int)sectionId).SectionName;
                ViewBag.sectionName = section_Name;

            }
            else if (gradeId != null)
            {
                model.StudentDTOs = studentService.GetStudentsByGrade((int)gradeId);
            }
            else if (sectionName != null)
            {
                var section = sectionService.GetSectionByName(sectionName);
                ViewBag.sectionName = sectionName;
                model.StudentDTOs = studentService.GetStudentBySection(section.Id);
            }
            else if (parentUsername != null)
            {
                var userId = userService.GetUserByUsername(parentUsername).Id;
                var parent = parentService.GetParentByUserId(userId);
                model.StudentDTOs = studentService.GetStudentByParent(parent.Id);
            }
            else if (certificateTypeName != null)
            {
                var certificateTypeId = certificateTypeService.GetCertificateType(certificateTypeName).Id;
                model.StudentDTOs = studentService.GetStudentBasedOnCertificates(certificateTypeId);
                ViewBag.certificateTypeName = certificateTypeName;
            }
            else if (studentStatus != null)
            {
                model.StudentDTOs = studentService.GetStudentList(studentStatus);
                ViewBag.studentStatus = studentStatus;
            }
            else
            {
                model.StudentDTOs = studentService.GetStudentList();
            }

            model.SectionDTOs = sectionService.GetSectionsWithGrade();

            return View(model);
        }

        public IActionResult StudentDetails(int studentId)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.StudentDTO = studentService.GetStudentDetails(studentId);
            model.ParentDTO = parentService.GetParent((int)model.StudentDTO.ParentId);
            return View(model);
        }

        public IActionResult StudentAdd(int parentId)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.ParentDTO = parentService.GetParent(parentId);
            return View(model);
        }
        [HttpPost]
        public IActionResult StudentAdd(StudentParentViewModel student)
        {
            if (ModelState.IsValid)
            {
                StudentDTO newStudent = student.StudentDTO;
                newStudent.ParentId = student.ParentDTO.Id;
                userService.GenerateUserAccount(newStudent.FirstName, newStudent.LastName, newStudent.IdentityNumber, "Öğrenci");
                // userService.NewUser(newStudent.IdentityNumber, "Öğrenci");

                UserDTO newUser = userService.GetUserByUsername(newStudent.IdentityNumber);
                newStudent.UserId = newUser.Id;

                studentService.NewStudent(newStudent);
                return RedirectToAction("RegistrationList");
            }
            return View(student);

        }

        public IActionResult StudentUpdate(int id)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.StudentDTO = studentService.GetStudent(id);
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult StudentUpdate(StudentParentViewModel student)
        {
            StudentDTO selectedStudent = studentService.GetStudent(student.StudentDTO.Id);
            studentService.UpdateStudent(student.StudentDTO);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult AssignSection(int id)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(id);
            model.SectionDTOs = sectionService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult AssignSection(StudentDetailsViewModel studentDetail)
        {
            StudentDTO student = studentService.GetStudent(studentDetail.StudentDTO.Id);

            if (student.SectionId != null)
            {
                var previousSection = sectionService.GetSection((int)student.SectionId);
                previousSection.NumberOfStudentsEnrolled -= 1;
                sectionService.UpdateSection(previousSection);
            }

            SectionDTO section = sectionService.GetSection((int)studentDetail.StudentDTO.SectionId);
            if (section.NumberOfStudentsEnrolled < section.StudentCapacity)
            {
                section.NumberOfStudentsEnrolled += 1;
                sectionService.UpdateSection(section);

                student.SectionId = studentDetail.StudentDTO.SectionId;
                studentService.UpdateStudent(student);
            }
            //else{ Hata mesajı döndür. }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult StudentAcademicInfo(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.ExamResultDTOs = examResultService.GetExamResultsOfStudent(studentId);
            model.CertificateDTOs = certificateService.GetCertificateList(studentId);
            return View(model);
        }
    }
}
