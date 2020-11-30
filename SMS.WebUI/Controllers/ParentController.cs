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
                parents = parentService.GetInstructorsParents(instructorUserName);
            }
            else
            {
                parents = parentService.GetAll();
            }
            return View(parents);
        }
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
