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
    public class CertificateController : Controller
    {
        private readonly ICertificateService certificateService;
        private readonly ICertificateTypeService certificateTypeService;
        private readonly IStudentService studentService;
        private readonly ISemesterService semesterService;
        private readonly IInstructorService instructorService;

        public CertificateController(ICertificateService _certificateService, ICertificateTypeService _certificateTypeService, IStudentService _studentService, ISemesterService _semesterService, IInstructorService _instructorService)
        {
            certificateService = _certificateService;
            certificateTypeService = _certificateTypeService;
            studentService = _studentService;
            semesterService = _semesterService;
            instructorService = _instructorService;
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Öğrenci, Veli")]
        public IActionResult CertificateList(int? studentId, int? semesterId)
        {
            CertificateViewModel model = new CertificateViewModel();
            if (studentId != null)
            {
                model.StudentDTO = studentService.GetStudent((int)studentId);
                if (semesterId != null)
                {
                    model.CertificateDTOs = certificateService.GetCertificateList((int)studentId, (int)semesterId);
                    model.SemesterDTO = semesterService.GetSemester((int)semesterId);
                }
                else
                {
                    model.SemesterDTO = semesterService.GetCurrentSemester(DateTime.Now);
                    model.CertificateDTOs = certificateService.GetCertificateList((int)studentId, model.SemesterDTO.Id);
                    //model.CertificateDTOs = certificateService.GetCertificateList((int)studentId);
                }
                model.SemesterDTOs = semesterService.GetAllSemestersOfStudent((int)studentId);
            }
            else
            {
                model.CertificateDTOs = certificateService.GetAll();
                model.SemesterDTOs = semesterService.GetAll();
            }
            return View(model);
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult CertificateAdd(int studentId)
        {
            CertificateViewModel model = new CertificateViewModel();
            model.CertificateTypeDTOs = certificateTypeService.GetAll();

            model.CertificateDTO = new CertificateDTO();
            model.CertificateDTO.StudentId = studentId;
            // model.StudentDTO = studentService.GetStudent(studentId);
            model.SemesterDTOs = semesterService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult CertificateAdd(CertificateViewModel model)
        {
            certificateService.NewCertificate(model.CertificateDTO);
            return RedirectToAction("CertificateList", new { studentId = model.CertificateDTO.StudentId });
            //return Redirect(Request.Headers["Referrer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult CertificateDelete(int certificateId, int studentId)
        {
            certificateService.DeleteCertificate(certificateId);
            return RedirectToAction("CertificateList", new { studentId = studentId });
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult CertificateUpdate(int certificateId)
        {
            CertificateViewModel model = new CertificateViewModel();
            model.CertificateDTO = certificateService.GetCertificate(certificateId);
            model.CertificateTypeDTOs = certificateTypeService.GetAll();
            model.SemesterDTOs = semesterService.GetAll();
            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult CertificateUpdate(CertificateViewModel model)
        {
            certificateService.UpdateCertificate(model.CertificateDTO);
            return RedirectToAction("CertificateList", new { studentId = model.CertificateDTO.StudentId });
        }

        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult CreateCertificate(int studentId, int semesterId)
        {
            CertificateViewModel model = new CertificateViewModel();
            ViewBag.schoolPrinciple = instructorService.GetInstructorByDuty("Okul Müdürü").FullName;
            model.CertificateDTO = certificateService.CreateCertificate(studentId, semesterId);
            model.StudentDTO = studentService.GetStudent(studentId);
            model.SemesterDTO = semesterService.GetSemester(semesterId);
            return PartialView(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Öğrenci, Veli")]
        public IActionResult CertificateDetail(int studentId, int certificateId)
        {
            CertificateViewModel model = new CertificateViewModel();
            ViewBag.schoolPrinciple = instructorService.GetInstructorByDuty("Okul Müdürü").FullName;
            model.CertificateDTO = certificateService.GetCertificate(certificateId);
            model.StudentDTO = studentService.GetStudent(studentId);
            return PartialView(model);
        }
    }
}
