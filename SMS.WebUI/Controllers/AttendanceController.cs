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
        private readonly ISemesterService semesterService;
        public AttendanceController(IAttendanceService _attendanceService, IStudentService _studentService, IAttendanceTypeService _attendanceTypeService, ISemesterService _semesterService)
        {
            attendanceService = _attendanceService;
            studentService = _studentService;
            attendanceTypeService = _attendanceTypeService;
            semesterService = _semesterService;
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli, Öğrenci")]
        public IActionResult AttendanceList(int studentId, int? semesterId)
        {
            StudentDetailsViewModel model = new StudentDetailsViewModel();
            model.StudentDTO = studentService.GetStudent(studentId);
            if (semesterId != null)
            {
                model.SemesterDTO = semesterService.GetSemester((int)semesterId);
                model.AttendanceDTOs = attendanceService.GetAttendanceOfStudent((int)studentId, (int)semesterId);
                ViewBag.TotalAbsenteeism = model.AttendanceDTOs.Count(z => z.AttendanceType.AttendanceTypeName == "Katılmadı");
                ViewBag.TotalSickLeave = model.AttendanceDTOs.Count(z => z.AttendanceType.AttendanceTypeName == "Raporlu" || z.AttendanceType.AttendanceTypeName == "Nöbetçi Öğrenci");
            }
            //else
            //{
            //    model.SemesterDTO = semesterService.GetCurrentSemester(DateTime.Now);
            //    model.AttendanceDTOs = attendanceService.GetAttendanceOfStudent((int)studentId, model.SemesterDTO.Id);
            //    ViewBag.TotalAbsenteeism = model.AttendanceDTOs.Count(z => z.AttendanceType.AttendanceTypeName == "Katılmadı");
            //    ViewBag.TotalSickLeave = model.AttendanceDTOs.Count(z => z.AttendanceType.AttendanceTypeName == "Raporlu" || z.AttendanceType.AttendanceTypeName == "Nöbetçi Öğrenci");
            //}
            model.SemesterDTOs = semesterService.GetAllSemestersOfStudent((int)studentId);
            model.AttendanceTypeDTOs = attendanceTypeService.GetAll();
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
            model.AttendanceDTO.SemesterId = semesterService.GetCurrentSemester(model.AttendanceDTO.DateTime).Id;
            attendanceService.NewAttendance(model.AttendanceDTO);
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
