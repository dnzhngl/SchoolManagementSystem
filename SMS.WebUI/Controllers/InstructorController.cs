using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IAttendanceService attendanceService;


        public InstructorController(IBranchService _branchService, IInstructorService _instructorService, IUserService _userService, ITimetableViewService _timetableViewService, IStudentService _studentService, ISectionService _sectionService, IAttendanceService _attendanceService)
        {
            instructorService = _instructorService;
            branchService = _branchService;
            userService = _userService;
            timetableViewService = _timetableViewService;
            studentService = _studentService;
            sectionService = _sectionService;
            attendanceService = _attendanceService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult InstructorList(int? id)
        {
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            //sadece if kısmını yeni ekledin. else kısmı vardı.
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

        public IActionResult InstructorAdd()
        {
            InstructorBranchViewModel model = new InstructorBranchViewModel();
            model.BranchDTOs = branchService.GetAll();
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult InstructorAdd(InstructorBranchViewModel instructor)
        {
            InstructorDTO newInstructor = instructor.InstructorDTO;
            UserDTO newUser = userService.GenerateUserAccount(newInstructor.FirstName, newInstructor.LastName, newInstructor.IdentityNumber, "Öğretmen");
            newInstructor.UserId = newUser.Id;

            instructorService.NewInstructor(newInstructor);

            return RedirectToAction("InstructorList");
        }

        public IActionResult InstructorDelete(int id)
        {
            int userId = (int)instructorService.GetInstructor(id).UserId;
            userService.DeleteUser(userId);

            instructorService.DeleteInstructor(id);
            return RedirectToAction("InstructorList");
        }

        public IActionResult InstructorUpdate(int id)
        {
            InstructorDTO selectedInstructor = instructorService.GetInstructor(id);
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            model.InstructorDTO = selectedInstructor;
            model.BranchDTO = branchService.GetBranch(selectedInstructor.BranchId);
            model.BranchDTOs = branchService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult InstructorUpdate(InstructorBranchViewModel instructor)
        {
            InstructorDTO selectedInstructor = instructor.InstructorDTO;
            selectedInstructor.BranchDTO = instructor.BranchDTO;
            instructorService.UpdateInstructor(selectedInstructor);
            return RedirectToAction("InstructorList");
        }

        public IActionResult InstructorDetails(int id)
        {
            InstructorDTO selectedInstructor = instructorService.GetInstructor(id);
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            model.InstructorDTO = selectedInstructor;
            model.InstructorDTO.BranchDTO = branchService.GetBranch(selectedInstructor.BranchId);
            return PartialView(model);
        }

        public IActionResult LecturedClasses(int? id, string? username)//Ahmet.Solmaz olarak geliyor isim.
        {
            List<TimetableViewDTO> ttmodel = new List<TimetableViewDTO>();
            InstructorDTO instructor = new InstructorDTO();
            if (id != null)
            {
                instructor = instructorService.GetInstructoreByUserId((int)id);
            }
            else if (username != null)
            {
                instructor = instructorService.GetInstructoreByUsername(username);
            }
            ttmodel = timetableViewService.GetTimetableGroupedByInstructor(instructor.Id);
            
            return View(ttmodel);
        }

        public IActionResult InstructorsStudents(string? sectionName, string? username)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            if (sectionName != null)
            {
                model.SectionDTO = sectionService.GetSectionByName(sectionName);
                model.StudentDTOs = studentService.GetStudentBySection(model.SectionDTO.Id);
            }
            else if (username != null)
            {
                var instructor = instructorService.GetInstructoreByUsername(username);
                model.StudentDTOs = studentService.GetStudentsByInstructor(instructor.Id);
                model.SectionDTOs = sectionService.GetAll();
            }
            foreach (StudentDTO student in model.StudentDTOs)
            {
                student.AttendanceDTOs = attendanceService.GetAttendanceOfStudent(student.Id);
            }
            return View(model);
        }
    }
}
