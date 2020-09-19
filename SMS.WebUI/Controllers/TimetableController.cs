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
        private readonly IStudentService studentService;

        private readonly ITimetableViewService timetableViewService;

        public TimetableController(ITimetableService _timetableService, ISubjectService _subjectService, IDayService _dayService, ILessonTimeService _lessonTimeService, IInstructorService _instructorService, ISectionService _sectionService, ITimetableViewService _timetableViewService, IClassroomService _classroomService, IGradeService _gradeService, IStudentService _studentService)
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
            studentService = _studentService;

        }

        public IActionResult TimetableIndex()
        {
            TimetableViewModel model = new TimetableViewModel();
            model.SectionDTOs = sectionService.GetAll();
            model.GradeDTOs = gradeService.GetAll();
            return View(model);
        }

        public IActionResult TimetableList(int? sectionId, string? username, int? classroomId) //sectionId
        {
            TimetableViewModel model = new TimetableViewModel();

            if (sectionId != null)
            {
                model.TimetableViewDTOs = timetableViewService.GetTimetableBySection((int)sectionId);
                model.SectionDTO = sectionService.GetSection((int)sectionId);
            }
            else if (username != null)
            {
                model.InstructorDTO = instructorService.GetInstructoreByUsername(username);
                // model.TimetableViewDTOs = timetableViewService.GetTimetableByInstructor((int)model.InstructorDTO.Id);

                model.TimetableViewDTOs = timetableViewService.GetTimetableGroupedByInstructor(model.InstructorDTO.Id);
            }
            else if (classroomId != null)
            {
                model.ClassroomDTO = classroomService.GetClassroom((int)classroomId);
                model.TimetableViewDTOs = timetableViewService.GetTimetableByClassroom((int)classroomId);
            }
            else
            {
                model.TimetableViewDTOs = timetableViewService.GetAll();
                model.SectionDTOs = sectionService.GetAll();
            }
            return View(model);
        }
        public IActionResult TimetableDesign(int? id, int? instructorId, string? studentUserName, string? instructorUserName, int? classroomId) //SectionId
        {
            TimetableViewModel model = new TimetableViewModel();

            model.DayDTOs = dayService.GetAll();
            model.LessonTimeDTOs = lessonTimeService.GetAll();
            if (id != null)
            {
                model.SectionDTO = sectionService.GetSection((int)id);
                model.TimetableViewDTOs = timetableViewService.GetTimetableBySection((int)id);
            }
            else if(instructorId != null | instructorUserName != null)
            {
                if (instructorUserName != null)
                {
                    instructorId = instructorService.GetInstructoreByUsername(instructorUserName).Id;
                }
                model.TimetableViewDTOs = timetableViewService.GetTimetableByInstructor((int)instructorId);
                model.InstructorDTO = instructorService.GetInstructor((int)instructorId);
            }
            else if (studentUserName != null)
            {
                var student = studentService.GetStudentByUsername(studentUserName);
                model.TimetableViewDTOs = timetableViewService.GetTimetableBySection((int)student.SectionId);
            }
            else if (classroomId != null)
            {
                model.ClassroomDTO = classroomService.GetClassroom((int)classroomId);
                model.TimetableViewDTOs = timetableViewService.GetTimetableByClassroom((int)classroomId);
            }
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
            var tt = timetableService.GetTimeTable(id);
            timetableService.DeleteTimeTable(id);

            return RedirectToAction("TimetableDesign", new { id = tt.SectionId });
        }

        public IActionResult TimetableUpdate(int ttId, int dayId, int lessonPeriodId)
        {
            TimetableViewModel model = new TimetableViewModel();
            model.TimeTableDTO = timetableService.GetTimeTable(ttId);
            model.TimetableViewDTO = timetableViewService.GetTimeTable(ttId);

            model.ClassroomDTOs = classroomService.GetAll();
            model.SubjectDTOs = subjectService.GetAll();
            model.InstructorDTOs = instructorService.GetInstructorNameWithBranch();
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult TimetableUpdate(TimetableViewModel model)
        {
            var tt = timetableService.GetTimeTable(model.TimeTableDTO.Id);
            tt.ClassroomId = model.TimeTableDTO.ClassroomId;
            tt.SubjectId = model.TimeTableDTO.SubjectId;
            tt.InstructorId = model.TimeTableDTO.InstructorId;

            timetableService.UpdateTimeTable(tt);
            
            return RedirectToAction("TimetableDesign", new { id = tt.SectionId });
        }

    }
}
