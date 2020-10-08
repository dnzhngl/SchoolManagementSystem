using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult ParentList(string? instructorUserName)
        {
            StudentParentViewModel model = new StudentParentViewModel();

            if (instructorUserName != null)
            {
                var instructor = instructorService.GetInstructorByUsername(instructorUserName);

                model.ParentDTOs = parentService.GetInstructorsParents(instructorUserName);
                model.StudentDTOs = studentService.GetStudentsOfInstructor(instructor.Id);
            }
            else
            {
                model.ParentDTOs = parentService.GetAll();
                model.StudentDTOs = studentService.GetAll();
            }
            return View(model);
        }

        public IActionResult ParentAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ParentAdd(StudentParentViewModel parent)
        {
            ParentDTO newParent = parent.ParentDTO;

            UserDTO newUser = userService.NewUser(newParent.IdentityNumber, "Veli");
            newParent.UserId = newUser.Id;

            newParent = parentService.NewParent(newParent);

            int parentId = newParent.Id;
            return RedirectToAction("StudentAdd", "Student", new { parentId });
        }

        public IActionResult ParentDelete(int id)
        {
            int userId = (int)parentService.GetParent(id).UserId;
            userService.DeleteUser(userId);

            parentService.DeleteParent(id);
            //return RedirectToAction("ParentList");
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult ParentUpdate(int id)
        {
            ParentDTO selectedParent = parentService.GetParent(id);
            StudentParentViewModel model = new StudentParentViewModel();
            model.ParentDTO = selectedParent;
            model.StudentDTOs = studentService.GetStudentByParent(id);
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ParentUpdate(StudentParentViewModel parent)
        {
            ParentDTO selectedParent = parent.ParentDTO;
            parentService.UpdateParent(selectedParent);
            return RedirectToAction("ParentList");
        }

        public IActionResult ParentsStudents(int? id, string? username)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            if (id != null)
            {
                model.StudentDTOs = studentService.GetStudentByParent((int)id);
            }
            else if (username != null)
            {
                var userparent = userService.GetUserByUsername(username);
                model.ParentDTO = parentService.GetParentByUserId(userparent.Id);
                model.StudentDTOs = studentService.GetStudentByParent((int)model.ParentDTO.Id);
            }
            foreach (var student in model.StudentDTOs)
            {
                if (student.SectionId != null)
                {
                    student.Section = sectionService.GetSection((int)student.SectionId); //SectionDTO
                }
                else
                {
                    student.Section = null; //SectionDTO
                }
            }
            //model.SectionDTOs = sectionService.GetAll();
            return PartialView(model);
        }

        public IActionResult ParentDetails(int id)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.ParentDTO = parentService.GetParent(id);
            model.StudentDTOs = studentService.GetStudentByParent(id);
            return View(model);
        }

    }
}
