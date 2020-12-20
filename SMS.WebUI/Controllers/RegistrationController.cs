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
    public class RegistrationController : Controller
    {
        private readonly IStudentService studentService;
        private readonly IParentService parentService;
        private readonly IUserService userService;
        public RegistrationController(IStudentService _studentService, IParentService _parentService, IUserService _userService)
        {
            studentService = _studentService;
            parentService = _parentService;
            userService = _userService;
        }
        public IActionResult RegistrationList()
        {
            var registrationList = studentService.GetAll().Where(z => z.SectionId == null);
            return View(registrationList);
        }
        public IActionResult RegistrationAdd()
        {
            StudentDTO student = new StudentDTO();
            student.SchoolNumber = studentService.GenerateStudentNumber();
            student.Parent = new ParentDTO();
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrationAdd(StudentDTO student)
        {
            if (ModelState.IsValid)
            {
                ParentDTO parent = parentService.GetParent(student.Parent.IdentityNumber, student.Parent.FirstName, student.Parent.LastName);

                if (parent == null)
                {
                    var parentUserId = userService.GenerateUserAccount(student.Parent.FirstName, student.Parent.LastName, student.Parent.IdentityNumber, "Veli").Id;
                    student.Parent.UserId = parentUserId;
                }
                else
                {
                    student.Parent = null;
                    student.ParentId = parent.Id;
                }
                var studentUserId = userService.GenerateUserAccount(student.FirstName, student.LastName, student.IdentityNumber, "Öğrenci", student.SchoolNumber).Id;
                student.UserId = studentUserId;

                studentService.NewStudent(student);

                var addedStudent = studentService.GetStudentByUsername(student.SchoolNumber);
                if (addedStudent != null)
                {
                    return RedirectToAction("RegistrationDetail", new { studentId = addedStudent.Id });
                }
            }
            return View(student);
        }
        public IActionResult RegistrationDetail(int studentId)
        {
            var student = studentService.GetStudent(studentId);
            if(student.Section == null)
            {
                student.Section = new SectionDTO();
                student.Section.Grade = new GradeDTO();
            }
            return View(student);
        }
        public IActionResult RegistrationDelete(int studentId)
        {
            var student = studentService.GetStudent(studentId);
            //var parent = parentService.GetParent(student.ParentId);
            var parent = parentService.GetParentWithStudents(student.ParentId);

            studentService.DeleteStudent(studentId);
            userService.DeleteUser((int)student.UserId);

            if (parent.Students.Count() == 1)
            {
                parentService.DeleteParent(student.ParentId);
                userService.DeleteUser((int)parent.UserId);
            }

            return RedirectToAction("RegistrationList");
        }

        //public IActionResult RegistrationUpdate(int studentId)
        //{
        //    var student = studentService.GetStudent(studentId);
        //    return View(student);
        //}
        //[HttpPost]
        //public IActionResult RegistrationUpdate(StudentDTO student)
        //{
        //    studentService.UpdateStudent(student);
        //    return RedirectToAction("RegistrationAdded", new { studentId = student.Id });
        //}
    }
}
