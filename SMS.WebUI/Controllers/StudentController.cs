using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IInstructorService instructorService;
        private readonly ISectionService sectionService;
        private readonly IGradeService gradeService;
        private readonly IUserService userService;
        private readonly ICertificateTypeService certificateTypeService;
        private readonly ICertificateService certificateService;
        private readonly IPostService postService;
        private readonly IExamResultService examResultService;
        private readonly IExamService examService;

        public StudentController(IStudentService _studentService, IParentService _parentService, ISectionService _sectionService, IGradeService _gradeService, IUserService _userService, ICertificateTypeService _certificateTypeService, IPostService _postService, IExamResultService _examResultService, ICertificateService _certificateService, IExamService _examService, IInstructorService _instructorService)
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
            examService = _examService;
            instructorService = _instructorService;
        }
        public IActionResult Index()
        {
            MainPageViewModel model = new MainPageViewModel();
            model.PostDTOs = postService.GetAll();
            model.UserDTO = userService.GetUserByUsername(this.User.Identity.Name);
            return View(model);
        }

        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult StudentDelete(int id)
        {
            var student = studentService.GetStudent(id);

            int userId = (int)student.UserId;
            userService.DeleteUser(userId);

            int parentId = student.ParentId;
            studentService.DeleteStudent(id);

            return RedirectToAction("ParentDelete", "Parent", new { id = parentId });
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult StudentList(int? sectionId, string? sectionName, string? certificateTypeName, string? studentStatus, int? gradeId)
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
            StudentDTO student = studentService.GetStudent(studentId);
            return View(student);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult StudentUpdate(int id)
        {
            //StudentParentViewModel model = new StudentParentViewModel();
            //model.StudentDTO = studentService.GetStudent(id);
            StudentDTO student = studentService.GetStudent(id);

            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult StudentUpdate(StudentDTO student)
        {
            if (ModelState.IsValid)
            {
                //StudentDTO selectedStudent = studentService.GetStudent(student.StudentDTO.Id);
                studentService.UpdateStudent(student);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return View(student);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult AssignSection(int id)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(id);
            model.SectionDTOs = sectionService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Yönetici")]
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
            else
            {
                return View(studentDetail);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult ChangeStudentStatus(int studentId)
        {
            StudentDTO student = studentService.GetStudent(studentId);
            return PartialView(student);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        [HttpPost]
        public IActionResult ChangeStudentStatus(StudentDTO student)
        {
            var studentStatus = student.StudentStatus;

            student = studentService.GetStudent(student.Id);
            student.StudentStatus = studentStatus;
            student.StudentStatusEditDate = DateTime.Now;
            studentService.UpdateStudent(student);

            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli, Öğrenci")]
        public IActionResult StudentAcademicInfo(int? studentId, string? studentUserName)
        {
            if (studentUserName != null)
            {
                studentId = studentService.GetStudentByUsername(studentUserName).Id;
            }
            StudentDTO student = studentService.GetStudent((int)studentId);
            return View(student);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli, Öğrenci")]
        public IActionResult StudentExamResults(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.ExamResultDTOs = examResultService.GetExamResultsOfStudent(studentId);
            if (User.IsInRole("Öğretmen"))
            {
                ViewBag.InstructorId = instructorService.GetInstructorByUsername(User.Identity.Name).Id;
            }

            return View(model);
        }


        #region Instead of this RegisterAdd and RegistrationList in RegisterController is in use.
        //public IActionResult StudentAdd(int parentId)
        //{
        //    StudentParentViewModel model = new StudentParentViewModel();
        //    model.ParentDTO = parentService.GetParent(parentId);
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult StudentAdd(StudentParentViewModel student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        StudentDTO newStudent = student.StudentDTO;
        //        newStudent.ParentId = student.ParentDTO.Id;
        //        userService.GenerateUserAccount(newStudent.FirstName, newStudent.LastName, newStudent.IdentityNumber, "Öğrenci");
        //        // userService.NewUser(newStudent.IdentityNumber, "Öğrenci");

        //        UserDTO newUser = userService.GetUserByUsername(newStudent.IdentityNumber);
        //        newStudent.UserId = newUser.Id;

        //        studentService.NewStudent(newStudent);
        //        return RedirectToAction("RegistrationList");
        //    }
        //    return View(student);
        //}

        #endregion


        //public IActionResult StudentExamsList(int studentId)
        //{
        //    StudentDetailsViewModel model = new StudentDetailsViewModel();

        //    model.ExamDTOs = examService.GetExamsByStudent(studentId);// 
        //    model.ExamResultDTOs = examResultService.GetExamResultsOfStudent(studentId);
        //    model.StudentDTO = studentService.GetStudent(studentId);


        //    if (User.IsInRole("Öğretmen"))
        //    {
        //        var user = userService.GetUserByUsername(this.User.Identity.Name);
        //        var instructor = instructorService.GetInstructorByUsername(user.UserName);

        //        ViewBag.userId = user.Id;
        //        ViewBag.instructorId = instructor.Id;

        //     //   model.ExamResultDTOs = examResultService.GetExamResultsOfStudent(studentId, instructor.Id);
        //    }

        //    return View(model);
        //}
        //public IActionResult StudentExamResults(int studentId, string? studentUserName, int? subjectId)
        //{
        //    StudentDetailsViewModel model = new StudentDetailsViewModel();
        //    if (studentUserName != null)
        //    {
        //        studentId = studentService.GetStudentByUsername(studentUserName).Id;
        //    }
        //    if (subjectId != null)
        //    {
        //        model.ExamDTOs = examService.GetExamBySubject((int)subjectId);
        //    }
        //    model.StudentDTO = studentService.GetStudent(studentId);
        //    model.ExamResultDTOs = examResultService.GetExamResultsOfStudent(studentId);
        //    return View(model);
        //}

    }
}
