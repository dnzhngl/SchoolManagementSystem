using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli, Öğrenci")]
        public IActionResult AttendanceList(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.AttendanceDTOs = attendanceService.GetAttendanceOfStudent(model.StudentDTO.Id);
            model.AttendanceTypeDTOs = attendanceTypeService.GetAll();
            model.StudentDTO.Attendances = model.AttendanceDTOs; //AttendanceDTOs
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult AttendanceAdd(int studentId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.AttendanceTypeDTOs = attendanceTypeService.GetAll();
            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult AttendanceAdd(StudentDetailsViewModel model)
        {
            var student = studentService.GetStudent(model.StudentDTO.Id);
            model.AttendanceDTO.StudentId = student.Id;
            attendanceService.NewAttendance(model.AttendanceDTO);
            string sectionN = sectionService.GetSection((int)student.SectionId).SectionName;
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult AttendanceDelete(int id)
        {
            attendanceService.DeleteAttendance(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        [HttpGet]
        public IActionResult AttendanceUpdate(int id)
        {
            var attendance = attendanceService.GetAttendance(id);
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.AttendanceDTO = attendance;
            model.StudentDTO = studentService.GetStudent(attendance.StudentId);

            return PartialView(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen")]
        public IActionResult AttendanceUpdate(StudentDetailsViewModel model)
        {
            attendanceService.UpdateAttendance(model.AttendanceDTO);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
