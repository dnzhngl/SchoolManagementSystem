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
    public class CertificateController : Controller
    {
        private readonly ICertificateService certificateService;
        private readonly ICertificateTypeService certificateTypeService;
        private readonly IStudentService studentService;
        private readonly ISemesterService semesterService;

        public CertificateController(ICertificateService _certificateService, ICertificateTypeService _certificateTypeService, IStudentService _studentService, ISemesterService _semesterService)
        {
            certificateService = _certificateService;
            certificateTypeService = _certificateTypeService;
            studentService = _studentService;
            semesterService = _semesterService;
        }

        public IActionResult CertificateList(int? studentId)
        {
            CertificateViewModel model = new CertificateViewModel();
            if (studentId != null)
            {
                model.StudentDTO = studentService.GetStudent((int)studentId);
                model.CertificateDTOs = certificateService.GetCertificateList((int)studentId);
            }
            else
            {
                model.CertificateDTOs = certificateService.GetAll();
            }
            return View(model);
        }

        public IActionResult CertificateAdd(int studentId)
        {
            CertificateViewModel model = new CertificateViewModel();
            model.CertificateTypeDTOs = certificateTypeService.GetAll();
            model.StudentDTO = studentService.GetStudent(studentId);
            model.SemesterDTOs = semesterService.GetAll();

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult CertificateAdd(CertificateViewModel model)
        {
            model.CertificateDTO.StudentId = model.StudentDTO.Id;
            certificateService.NewCertificate(model.CertificateDTO);

            return RedirectToAction("CertificateList", new { studentId = model.StudentDTO.Id });
        }
        public IActionResult CertificateDelete(int certificateId, int studentId)
        {
            certificateService.DeleteCertificate(certificateId);
            return RedirectToAction("CertificateList", new { studentId = studentId });
        }
        public IActionResult CertificateUpdate(int certificateId)
        {
            CertificateViewModel model = new CertificateViewModel();
            model.CertificateDTO = certificateService.GetCertificate(certificateId);
            model.CertificateTypeDTOs = certificateTypeService.GetAll();
            model.SemesterDTOs = semesterService.GetAll();
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult CertificateUpdate(CertificateViewModel model)
        {
            certificateService.UpdateCertificate(model.CertificateDTO);
            return RedirectToAction("CertificateList", new { studentId = model.CertificateDTO.StudentId });
        }
    }
}
