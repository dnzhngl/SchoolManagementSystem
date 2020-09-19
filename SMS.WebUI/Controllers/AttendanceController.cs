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
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService attendanceService;
        private readonly IAttendanceTypeService attendanceTypeService;
        private readonly IStudentService studentService;
        private readonly ISectionService sectionService;
        public AttendanceController(IAttendanceService _attendanceService, IStudentService _studentService, IAttendanceTypeService _attendanceTypeService, ISectionService _sectionService)
        {
            attendanceService = _attendanceService;
            studentService = _studentService;
            attendanceTypeService = _attendanceTypeService;
            sectionService = _sectionService;
        }
        public IActionResult AttendanceList(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.AttendanceDTOs = attendanceService.GetAttendanceOfStudent(model.StudentDTO.Id);
            model.AttendanceTypeDTOs = attendanceTypeService.GetAll();
            model.StudentDTO.Attendances = model.AttendanceDTOs; //AttendanceDTOs
            return PartialView(model);
        }
        [HttpGet]
        public IActionResult AttendanceAdd(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.AttendanceTypeDTOs = attendanceTypeService.GetAll();
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult AttendanceAdd(StudentDetailsViewModel model)
        {
            var student = studentService.GetStudent(model.StudentDTO.Id);
            model.AttendanceDTO.StudentId = student.Id;
            attendanceService.NewAttendance(model.AttendanceDTO);
            string sectionN = sectionService.GetSection((int)student.SectionId).SectionName;
            return RedirectToAction("InstructorsStudents", "Instructor", new { sectionName = sectionN });
        }
    }
}
