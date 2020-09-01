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
    public class TimetableController : Controller
    {
        private readonly ITimetableService timetableService;
        private readonly ISubjectService subjectService;
        private readonly IDayService dayService;
        private readonly ILessonTimeService lessonTimeService;
        private readonly IInstructorService instructorService;
        private readonly ISectionService sectionService;
        private readonly IClassroomService classroomService;
        private readonly IGradeService gradeService;

        private readonly ITimetableViewService timetableViewService;

        public TimetableController(ITimetableService _timetableService, ISubjectService _subjectService, IDayService _dayService, ILessonTimeService _lessonTimeService, IInstructorService _instructorService, ISectionService _sectionService, ITimetableViewService _timetableViewService, IClassroomService _classroomService, IGradeService _gradeService)
        {
            timetableService = _timetableService;
            subjectService = _subjectService;
            dayService = _dayService;
            lessonTimeService = _lessonTimeService;
            sectionService = _sectionService;
            instructorService = _instructorService;
            classroomService = _classroomService;
            gradeService = _gradeService;

            timetableViewService = _timetableViewService;
        }

        public IActionResult TimetableIndex()
        {
            TimetableViewModel model = new TimetableViewModel();
            model.SectionDTOs = sectionService.GetAll();
            model.GradeDTOs = gradeService.GetAll();
            return View(model);
        }

        public IActionResult TimetableList(int? id)
        {
            TimetableViewModel model = new TimetableViewModel();

            if (id != null)
            {
                model.TimetableViewDTOs = timetableViewService.GetTimetableBySection((int)id);
                model.SectionDTO = sectionService.GetSection((int)id);
            }
            else
            {
                model.TimetableViewDTOs = timetableViewService.GetAll();
                model.SectionDTOs = sectionService.GetAll();
            }
            return View(model);
        }
        public IActionResult TimetableDesign(int id) //SectionId
        {
            TimetableViewModel model = new TimetableViewModel();

            model.SectionDTO = sectionService.GetSection((int)id);
            model.DayDTOs = dayService.GetAll();
            model.LessonTimeDTOs = lessonTimeService.GetAll();
            model.TimetableViewDTOs = timetableViewService.GetTimetableBySection((int)id);
            return View(model);
        }
        public IActionResult TimetableAdd(int? sectionId, int? dayId, int? lessonPeriodId)
        {
            TimetableViewModel model = new TimetableViewModel();
            if (sectionId != null)
            {
                model.SectionDTO = sectionService.GetSection((int)sectionId);
                model.TimetableViewDTO = new TimetableViewDTO();
                model.TimetableViewDTO.SectionName = model.SectionDTO.SectionName;
                if (dayId != null && lessonPeriodId != null)
                {
                    model.DayDTO = dayService.GetDay((int)dayId);
                    model.LessonTimeDTO = lessonTimeService.GetLessonTime((int)lessonPeriodId);
                    model.TimetableViewDTO.DayName = model.DayDTO.DayName;
                    model.TimetableViewDTO.LessonPeriod = model.LessonTimeDTO.LessonPeriod;
                }
                else
                {
                    model.DayDTOs = dayService.GetAll();
                    model.LessonTimeDTOs = lessonTimeService.GetAll();
                }
            }
            else
            {
                model.SectionDTOs = sectionService.GetAll();
                model.DayDTOs = dayService.GetAll();
                model.LessonTimeDTOs = lessonTimeService.GetAll();
            }
            model.ClassroomDTOs = classroomService.GetAll();
            model.SubjectDTOs = subjectService.GetAll();
            model.InstructorDTOs = instructorService.GetInstructorNameWithBranch();

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult TimetableAdd(TimetableViewModel model)
        {
            TimetableViewDTO ttView = model.TimetableViewDTO;

            TimetableDTO newTimetable = new TimetableDTO();
            newTimetable.ClassroomId = Convert.ToInt32(ttView.ClassroomName);
            newTimetable.SubjectId = Convert.ToInt32(ttView.SubjectName);
            newTimetable.InstructorId = Convert.ToInt32(ttView.Instructor);
            try
            {
                newTimetable.SectionId = Convert.ToInt32(ttView.SectionName);
                newTimetable.DayId = Convert.ToInt32(ttView.DayName);
                newTimetable.LessonTimeId = Convert.ToInt32(ttView.LessonPeriod);
            }
            catch
            {
                newTimetable.SectionId = sectionService.GetSectionByName(ttView.SectionName).Id;
                newTimetable.DayId = dayService.GetDayByName(ttView.DayName).Id;
                newTimetable.LessonTimeId = lessonTimeService.GetLessonTimeByPeriod(ttView.LessonPeriod).Id;
            }
            timetableService.NewTimeTable(newTimetable);

            return RedirectToAction("TimetableDesign", new { id = newTimetable.SectionId });
        }

        public IActionResult TimetableDelete(int id)
        {
            timetableService.DeleteTimeTable(id);
            return RedirectToAction("TimetableList");
        }

        public IActionResult TimetableUpdate(int id)
        {
            TimetableViewModel model = new TimetableViewModel();
            model.TimetableViewDTO = timetableViewService.GetTimeTable(id);
            model.TimeTableDTO = timetableService.GetTimeTable(id);
            model.TimeTableDTO.ClassroomDTO = classroomService.GetClassroom(model.TimeTableDTO.ClassroomId);
            model.TimeTableDTO.SubjectDTO = subjectService.GetSubjectByName(model.TimetableViewDTO.SubjectName);
            model.TimeTableDTO.InstructorDTO = instructorService.GetInstructorByName(model.TimetableViewDTO.Instructor);
            model.TimeTableDTO.SectionDTO = sectionService.GetSectionByName(model.TimetableViewDTO.SectionName);
            model.TimeTableDTO.DayDTO = dayService.GetDayByName(model.TimetableViewDTO.DayName);
            model.TimeTableDTO.LessonTimeDTO = lessonTimeService.GetLessonTimeByPeriod(model.TimetableViewDTO.LessonPeriod);

            model.ClassroomDTOs = classroomService.GetAll();
            model.SubjectDTOs = subjectService.GetAll();
            model.InstructorDTOs = instructorService.GetInstructorNameWithBranch();
            model.DayDTOs = dayService.GetAll();
            model.LessonTimeDTOs = lessonTimeService.GetAll();

            //model.ClassroomDTO = classroomService.GetClassroomByName(model.TimetableViewDTO.ClassroomName);
            //model.SubjectDTO = subjectService.GetSubjectByName(model.TimetableViewDTO.SubjectName);
            //model.InstructorDTO = instructorService.GetInstructorByName(model.TimetableViewDTO.Instructor);
            //model.SectionDTO = sectionService.GetSectionByName(model.TimetableViewDTO.SectionName);
            //model.DayDTO = dayService.GetDayByName(model.TimetableViewDTO.DayName);
            //model.LessonTimeDTO = lessonTimeService.GetLessonTimeByPeriod(model.TimetableViewDTO.LessonPeriod);

            return PartialView(model);
        }
        [HttpPost]
        public IActionResult TimetableUpdate(TimetableViewModel model)
        {
            return RedirectToAction("TimetableDesign", new { id = model.SectionDTO.Id });
        }

    }
}
