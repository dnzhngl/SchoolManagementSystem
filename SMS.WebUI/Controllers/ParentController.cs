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

        public ParentController(IStudentService _studentService, IParentService _parentService, ISectionService _sectionService)
        {
            studentService = _studentService;
            parentService = _parentService;
            sectionService = _sectionService;
        }
        public IActionResult ParentList()
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.ParentDTOs = parentService.GetAll();
            model.StudentDTOs = studentService.GetAll();

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
            newParent = parentService.NewParent(newParent);

            int parentId = newParent.Id;

            return RedirectToAction("StudentAdd","Student", new { parentId });
        }
        
        public IActionResult ParentDelete(int id)
        {
            parentService.DeleteParent(id);
            return RedirectToAction("ParentList");
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

        public IActionResult ParentDetails(int id)
        {
            StudentParentViewModel model = new StudentParentViewModel();
            model.StudentDTOs = studentService.GetStudentByParent(id);
            foreach (var student in model.StudentDTOs)
            {
                if (student.SectionId != null)
                {
                    student.SectionDTO = sectionService.GetSection((int)student.SectionId);
                }
                else
                {
                    student.SectionDTO = null;
                }
            } 

            //model.SectionDTOs = sectionService.GetAll();
            return PartialView(model);
        }
        //ParentDetails Sayfasından ilgili öğrenciye gidebilmek için
        //public IActionResult ParentDetails(StudentDTO student)
        //{
        //    int id = student.Id;
        //    return RedirectToAction("StudentList", "Student", id);
        //}

    }
}
