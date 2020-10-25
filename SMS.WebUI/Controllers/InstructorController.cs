using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorService instructorService;
        private readonly IBranchService branchService;
        private readonly IUserService userService;
        private readonly ITimetableViewService timetableViewService;
        private readonly IStudentService studentService;
        private readonly ISectionService sectionService;
        private readonly IPostService postService;
        private readonly IAttendanceService attendanceService;
        private readonly ISubjectService subjectService;


        public InstructorController(IBranchService _branchService, IInstructorService _instructorService, IUserService _userService, ITimetableViewService _timetableViewService, IStudentService _studentService, ISectionService _sectionService, IAttendanceService _attendanceService, IPostService _postService, ISubjectService _subjectService)
        {
            instructorService = _instructorService;
            branchService = _branchService;
            userService = _userService;
            timetableViewService = _timetableViewService;
            studentService = _studentService;
            sectionService = _sectionService;
            attendanceService = _attendanceService;
            postService = _postService;
            subjectService = _subjectService;
        }

        public IActionResult Index()
        {
            MainPageViewModel model = new MainPageViewModel();
            model.PostDTOs = postService.GetAll();
            model.UserDTO = userService.GetUserByUsername(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult InstructorList(int? id)
        {
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            if (id != null)
            {
                model.InstructorDTOs = instructorService.GetAllInstructorsBasedOnBranch((int)id);
            }
            else
            {
                model.InstructorDTOs = instructorService.GetAll();
            }
            model.BranchDTOs = branchService.GetAll();

            return View(model);

        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult InstructorAdd()
        {
            InstructorBranchViewModel model = new InstructorBranchViewModel();
            model.BranchDTOs = branchService.GetAll();
            return View(model);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        [HttpPost]
        public IActionResult InstructorAdd(InstructorBranchViewModel instructor)
        {
            if (ModelState.IsValid)
            {
                InstructorDTO newInstructor = instructor.InstructorDTO;
                UserDTO newUser = userService.GenerateUserAccount(newInstructor.FirstName, newInstructor.LastName, newInstructor.IdentityNumber, "Öğretmen");
                newInstructor.UserId = newUser.Id;

                instructorService.NewInstructor(newInstructor);
                return Redirect(Request.Headers["Referer"].ToString());

            }
            instructor.BranchDTOs = branchService.GetAll();
            return View(instructor);
            //return RedirectToAction("InstructorList");
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult InstructorDelete(int id)
        {
            int userId = (int)instructorService.GetInstructor(id).UserId;
            userService.DeleteUser(userId);

            instructorService.DeleteInstructor(id);
            return RedirectToAction("InstructorList");
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult InstructorUpdate(int id)
        {
            InstructorDTO selectedInstructor = instructorService.GetInstructor(id);
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            model.InstructorDTO = selectedInstructor;
            model.BranchDTO = branchService.GetBranch(selectedInstructor.BranchId);
            model.BranchDTOs = branchService.GetAll();

            return PartialView(model);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        [HttpPost]
        public IActionResult InstructorUpdate(InstructorBranchViewModel instructor)
        {
            InstructorDTO selectedInstructor = instructor.InstructorDTO;
            selectedInstructor.Branch = instructor.BranchDTO;
            instructorService.UpdateInstructor(selectedInstructor);
            return RedirectToAction("InstructorList");
        }
        public IActionResult InstructorDetails(int id)
        {
            InstructorDTO selectedInstructor = instructorService.GetInstructor(id);
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            model.InstructorDTO = selectedInstructor;
            model.InstructorDTO.Branch = branchService.GetBranch(selectedInstructor.BranchId);
            return View(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult LecturedClasses(int? id, string? instructorUsername)//Ahmet.Solmaz olarak geliyor isim.
        {
            List<TimetableViewDTO> ttmodel = new List<TimetableViewDTO>();
            InstructorDTO instructor = new InstructorDTO();
            if (id != null)
            {
                instructor = instructorService.GetInstructoreByUserId((int)id);
            }
            else if (instructorUsername != null)
            {
                instructor = instructorService.GetInstructorByUsername(instructorUsername);
            }
            ttmodel = timetableViewService.GetTimetableGroupedByInstructor(instructor.Id);

            return View(ttmodel);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult InstructorsStudents(string? sectionName,string? subjectName, string? instructorUsername)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            if (sectionName != null && subjectName != null)
            {
                model.SectionDTO = sectionService.GetSectionByName(sectionName);
                model.StudentDTOs = studentService.GetStudentsIncludes(model.SectionDTO.Id);
                ViewBag.SubjectId = subjectService.GetSubject(subjectName).Id;
                ViewBag.SubjectName = subjectName;
            }
            else if (instructorUsername != null)
            {
                var instructor = instructorService.GetInstructorByUsername(instructorUsername);
                model.StudentDTOs = studentService.GetStudentsOfInstructor(instructor.Id);
            }

            return View(model);
        }
    }
}
