using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IStudentSchoolReportViewService schoolReportService;
        private readonly IAttendanceService attendanceService;
        private readonly ISemesterService semesterService;

        public StudentController(IStudentService _studentService, IParentService _parentService, ISectionService _sectionService, IGradeService _gradeService, IUserService _userService, ICertificateTypeService _certificateTypeService, IPostService _postService, IExamResultService _examResultService, ICertificateService _certificateService, IExamService _examService, IInstructorService _instructorService, IStudentSchoolReportViewService _schoolReportService, IAttendanceService _attendanceService, ISemesterService _semesterService)
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
            schoolReportService = _schoolReportService;
            attendanceService = _attendanceService;
            semesterService = _semesterService;
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
        public IActionResult StudentList(int? sectionId, int? gradeId, string sectionName = "", string certificateTypeName = "", string studentStatus = "")
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
            else if (sectionName != "")
            {
                var section = sectionService.GetSectionByName(sectionName);
                ViewBag.sectionName = sectionName;
                model.StudentDTOs = studentService.GetStudentBySection(section.Id);
            }
            else if (certificateTypeName != "")
            {
                var certificateTypeId = certificateTypeService.GetCertificateType(certificateTypeName).Id;
                model.StudentDTOs = studentService.GetStudentBasedOnCertificates(certificateTypeId);
                ViewBag.certificateTypeName = certificateTypeName + " Belgesi Alan";
            }
            else if (studentStatus != "")
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
                studentService.UpdateStudent(student);
                return RedirectToAction("StudentDetails", new {studentId = student.Id });
                //return Redirect(Request.Headers["Referer"].ToString());
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
        public IActionResult StudentAcademicInfo(int? studentId, string studentUserName = "")
        {
            if (studentUserName != "")
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

        public IActionResult CreateSchoolReport(int studentId)
        {

            var model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.SemesterDTO = semesterService.GetCurrentSemester(DateTime.Now);

            var schoolReport = schoolReportService.CreateSchoolReport(studentId);
            model.StudentSchoolReportViewDTOs1 = schoolReport.Where(z => z.SemesterName == "I. Dönem").ToList();
            model.StudentSchoolReportViewDTOs2 = schoolReport.Where(z => z.SemesterName == "II. Dönem").ToList();

            ViewBag.schoolPrinciple = instructorService.GetInstructorByDuty("Okul Müdürü").FullName;
            ViewBag.vicePrinciple = instructorService.GetInstructorByDuty("Müdür Yardımcısı").FullName;
            ViewBag.advisoryTeacher = instructorService.GetInstructor((int)model.StudentDTO.Section.AdvisoryTeacherId).FullName;


            if (model.SemesterDTO.SemesterName == "I. Dönem")
            {
                model.AttendanceDTOs = attendanceService.GetAttendanceOfStudent(studentId, model.SemesterDTO.Id);

                ViewBag.attendanceExcuse1 = model.AttendanceDTOs.Where(z => z.AttendanceType.AttendanceTypeName == "Raporlu" || z.AttendanceType.AttendanceTypeName == "Nobetçi Öğrenci").Count();
                ViewBag.attendance1 = model.AttendanceDTOs.Where(z => z.AttendanceType.AttendanceTypeName == "Katılmadı").Count();
            }
            else if (model.SemesterDTO.SemesterName == "II. Dönem")
            {
                model.SemesterDTOs = semesterService.GetSemestersByAcademicYear(model.SemesterDTO.AcademicYear);
                var firstSemesterId = model.SemesterDTOs.FirstOrDefault(z => z.AcademicYear == model.SemesterDTO.AcademicYear && z.SemesterName == "I. Dönem").Id;

                model.AttendanceDTOs = attendanceService.GetAttendanceOfStudent(studentId, firstSemesterId);
                model.AttendanceDTOs2 = attendanceService.GetAttendanceOfStudent(studentId, model.SemesterDTO.Id);

                ViewBag.attendanceExcuse2 = model.AttendanceDTOs2.Where(z => z.AttendanceType.AttendanceTypeName == "Raporlu" || z.AttendanceType.AttendanceTypeName == "Nobetçi Öğrenci").Count();
                ViewBag.attendance2 = model.AttendanceDTOs2.Where(z => z.AttendanceType.AttendanceTypeName == "Katılmadı").Count();
            }

            //var certificate = certificateService.CreateCertificate(studentId, model.SemesterDTO.Id);
            //certificateService.NewCertificate(certificate);

            return View(model);
        }



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
