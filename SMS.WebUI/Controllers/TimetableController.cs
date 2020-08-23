using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.WebUI.Models;

namespace SMS.WebUI.Controllers
{
    public class TimetableController : Controller
    {
        private readonly ITimetableService timetableService;
        private readonly ISubjectService subjectService;
        private readonly IDayService dayService;
        private readonly ILessonTimeService lessonTimeService;
        private readonly IInstructorService instructorService;
        private readonly ISectionService sectionService;

        public TimetableController(ITimetableService _timetableService, ISubjectService _subjectService, IDayService _dayService, ILessonTimeService _lessonTimeService, IInstructorService _instructorService, ISectionService _sectionService)
        {
            timetableService = _timetableService;
            subjectService = _subjectService;
            dayService = _dayService;
            lessonTimeService = _lessonTimeService;
            sectionService = _sectionService;
            instructorService = _instructorService;
        }
        public IActionResult AddTimetable(int id)
        {
            TimetableViewModel model = new TimetableViewModel();
            model.SectionDTO = sectionService.GetSection(id);
            model.InstructorDTOs = instructorService.GetAll();
            model.DayDTOs = dayService.GetAll();
            model.LessonTimeDTOs = lessonTimeService.GetAll();
            model.SubjectDTOs = subjectService.GetAll();

            return View(model);
        }
    }
}
