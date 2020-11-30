using Microsoft.EntityFrameworkCore.Internal;
using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class StudentSchoolReportViewService : IStudentSchoolReportViewService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<StudentSchoolReportView> schoolReportRepo;
        public StudentSchoolReportViewService(IUnitOfWork _uow)
        {
            uow = _uow;
            schoolReportRepo = uow.GetRepository<StudentSchoolReportView>();
        }

        public List<StudentSchoolReportViewDTO> CreateSchoolReport(int studentId)
        {
            var vgList = schoolReportRepo.GetAll().Where(z => z.Id == studentId);
            return MapperFactory.CurrentMapper.Map<List<StudentSchoolReportViewDTO>>(vgList);
        }

        public List<StudentSchoolReportViewDTO> GetAll()
        {
            var list = schoolReportRepo.GetAll();
            return MapperFactory.CurrentMapper.Map<List<StudentSchoolReportViewDTO>>(list);

        }

        public IQueryable YearEndSubjectReport(int studentId)
        {
            var vgList = schoolReportRepo.GetAll().Where(z => z.Id == studentId);

            var query = vgList.GroupBy(z => new
            {
                z.Id,
                z.SchoolNumber,
                z.FirstName,
                z.LastName,
                z.MainSubjectName,
                z.WeeklyCourseHours,
                z.AcademicYear
            }).Select(x => new
            {
                x.Key.Id,
                x.Key.SchoolNumber,
                x.Key.FirstName,
                x.Key.LastName,
                YearEndMark = Math.Round(x.Average(z => z.AvgMark)),
                YearEndMarkNumeral = Math.Round(x.Average(z => z.AvgMarkNumeral)),
                YearEndGPA = Math.Round(x.Average(z => z.GPA)),
                YearEndGPANumeral = Math.Round(x.Average(z => z.GpaNumeral)),
                x.Key.MainSubjectName,
                x.Key.WeeklyCourseHours,
                x.Key.AcademicYear
            });

            return query;
        }

    }
}
