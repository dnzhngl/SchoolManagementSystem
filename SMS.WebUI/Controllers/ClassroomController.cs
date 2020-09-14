using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;

namespace SMS.WebUI.Controllers
{
    public class ClassroomController : Controller
    {
        private readonly IClassroomService classroomService;
        public ClassroomController(IClassroomService _classroomService)
        {
            classroomService = _classroomService;
        }
        public IActionResult ClassroomList()
        {
            return View(classroomService.GetAll());
        }
        public IActionResult ClassroomAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult ClassroomAdd(ClassroomDTO classroom)
        {
            classroomService.NewClassroom(classroom);
            return RedirectToAction("ClassroomList");
        }
        public IActionResult ClassroomDelete(int id)
        {
            classroomService.DeleteClassroom(id);
            return RedirectToAction("ClassroomList");
        }
        public IActionResult ClassroomUpdate(int id)
        {
            ClassroomDTO selectedClassroom = classroomService.GetClassroom(id);
            return PartialView(selectedClassroom);
        }
        [HttpPost]
        public IActionResult ClassroomUpdate(ClassroomDTO classroom)
        {
            classroomService.UpdateClassroom(classroom);
            return RedirectToAction("ClassroomList");
        }
    }
}
