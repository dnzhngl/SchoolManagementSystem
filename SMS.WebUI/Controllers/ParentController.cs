using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class ParentController : Controller
    {
        private readonly IParentService parentService;
        private readonly IStudentService studentService;
        private readonly ISectionService sectionService;
        private readonly IUserService userService;
        private readonly IInstructorService instructorService;

        public ParentController(IStudentService _studentService, IParentService _parentService, ISectionService _sectionService, IUserService _userService, IInstructorService _instructorService)
        {
            studentService = _studentService;
            parentService = _parentService;
            sectionService = _sectionService;
            userService = _userService;
            instructorService = _instructorService;
        }
        [Authorize(Roles = "Veli")]
        public IActionResult Index(int? userId, string? userName)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            if (userId != null)
            {
                model.ParentDTO = parentService.GetParentByUserId((int)userId);
                model.StudentDTOs = studentService.GetStudentByParent((int)model.ParentDTO.Id);
            }
            else if (userName != null)
            {
                var parentUserId = userService.GetUserByUsername(userName).Id;
                model.ParentDTO = parentService.GetParentByUserId(parentUserId);
                model.StudentDTOs = studentService.GetStudentByParent((int)model.ParentDTO.Id);
            }

            return View(model);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli")]
        public IActionResult ParentList(string? instructorUserName)
        {
            List<ParentDTO> parents = new List<ParentDTO>();

            if (instructorUserName != null)
            {
                // var instructor = instructorService.GetInstructorByUsername(instructorUserName);

                parents = parentService.GetInstructorsParents(instructorUserName);
                // model.StudentDTOs = studentService.GetStudentsOfInstructor(instructor.Id);
            }
            else
            {
                parents = parentService.GetAll();
                //model.StudentDTOs = studentService.GetAll();
            }
            return View(parents);
        }
        #region Instead of this RegisterAdd and RegistrationList in RegisterController is in use.
        //public IActionResult ParentAdd()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ParentAdd(StudentParentViewModel parent)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ParentDTO newParent = parent.ParentDTO;

        //        // userService.NewUser(newParent.IdentityNumber, "Veli");
        //        UserDTO newUser = userService.GenerateUserAccount(newParent.FirstName, newParent.LastName, newParent.IdentityNumber, "Veli");
        //        newParent.UserId = newUser.Id;

        //        newParent = parentService.NewParent(newParent);

        //        int parentId = newParent.Id;
        //        return RedirectToAction("StudentAdd", "Student", new { parentId });
        //    }
        //    return View(parent);
        //}
        #endregion
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult ParentDelete(int id)
        {
            int userId = (int)parentService.GetParent(id).UserId;
            parentService.DeleteParent(id);
            userService.DeleteUser(userId);
            
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult ParentUpdate(int id)
        {
            ParentDTO selectedParent = parentService.GetParent(id);
            return View(selectedParent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Yönetici")]
        public IActionResult ParentUpdate(ParentDTO parent)
        {
            if (ModelState.IsValid)
            {
                parentService.UpdateParent(parent);
                return RedirectToAction("ParentList");
            }
            return View(parent);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli")]
        public IActionResult ParentDetails(int id)
        {
            ParentDTO parent = parentService.GetParent(id);
            return View(parent);
        }
        [Authorize(Roles = "Admin, Yönetici, Öğretmen, Veli")]
        public IActionResult ParentsStudents(int parentId)
        {
            List<StudentDTO> students = studentService.GetStudentByParent(parentId);
            return PartialView(students);
        }
    }
}
