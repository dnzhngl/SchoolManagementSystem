using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.Model;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorService instructorService;
        private readonly IBranchService branchService;
        public InstructorController(IBranchService _branchService, IInstructorService _instructorService)
        {
            instructorService = _instructorService;
            branchService = _branchService;
        }
        public IActionResult InstructorList(int? id)
        {
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            //sadece if kısmını yeni ekledin. else kısmı vardı.
            if (id != null)
            {
                model.InstructorDTOs = instructorService.GetAllInstructorsBasedOnBranch((int)id);
            }
            else
            {
                model.InstructorDTOs = instructorService.GetAll();
            }
            model.BranchDTOs = branchService.GetAll();

            return View(model);

        }

        public IActionResult InstructorAdd()
        {
            InstructorBranchViewModel model = new InstructorBranchViewModel();
            model.BranchDTOs = branchService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult InstructorAdd(InstructorBranchViewModel instructor)
        {
            InstructorDTO newInstructor = instructor.InstructorDTO;
            instructorService.NewInstructor(newInstructor);

            return RedirectToAction("InstructorList");
        }

        public IActionResult InstructorDelete(int id)
        {
            instructorService.DeleteInstructor(id);
            return RedirectToAction("InstructorList");
        }

        public IActionResult InstructorUpdate(int id)
        {
            InstructorDTO selectedInstructor = instructorService.GetInstructor(id);
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            model.InstructorDTO = selectedInstructor;
            model.BranchDTO = branchService.GetBranch(selectedInstructor.BranchId);
            model.BranchDTOs = branchService.GetAll();

            return View(model);
        }
        [HttpPost]
        public IActionResult InstructorUpdate(InstructorBranchViewModel instructor)
        {
            InstructorDTO selectedInstructor = instructor.InstructorDTO;
            selectedInstructor.BranchDTO = instructor.BranchDTO;
            instructorService.UpdateInstructor(selectedInstructor);
            return RedirectToAction("InstructorList");
        }

        public IActionResult InstructorDetails(int id)
        {
            InstructorDTO selectedInstructor = instructorService.GetInstructor(id);
            InstructorBranchViewModel model = new InstructorBranchViewModel();

            model.InstructorDTO = selectedInstructor;
            model.InstructorDTO.BranchDTO = branchService.GetBranch(selectedInstructor.BranchId);
            return View(model);
        }

    }
}
