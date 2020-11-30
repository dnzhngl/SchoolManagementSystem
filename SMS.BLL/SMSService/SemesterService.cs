using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Semester> semesterRepo;

        public SemesterService(IUnitOfWork _uow)
        {
            uow = _uow;
            semesterRepo = uow.GetRepository<Semester>();        
        }
        public bool DeleteSemester(int id)
        {
            try
            {
                var selectedSemester = semesterRepo.Get(z => z.Id == id);
                semesterRepo.Delete(selectedSemester);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SemesterDTO> GetAll()
        {
            var semesterList = semesterRepo.GetAll().OrderBy(z => z.SemesterBeginning).ToList();
            return MapperFactory.CurrentMapper.Map<List<SemesterDTO>>(semesterList);
        }

        public SemesterDTO GetCurrentSemester(DateTime time)
        {
           // var currentSemester = semesterRepo.Get(z => z.SemesterBeginning > time && time > z.SemesterEnd);
            var currentSemester = semesterRepo.Get(z => z.SemesterBeginning < time && time < z.SemesterEnd);
            return MapperFactory.CurrentMapper.Map<SemesterDTO>(currentSemester);
        }

        public SemesterDTO GetSemester(int id)
        {
            var selectedSemester = semesterRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<SemesterDTO>(selectedSemester);
        }

        public List<SemesterDTO> GetSemestersByAcademicYear(string academicYear)
        {
            var semesters = semesterRepo.GetAll().Where(z => z.AcademicYear == academicYear);
            return MapperFactory.CurrentMapper.Map<List<SemesterDTO>>(semesters);
        }

        public SemesterDTO NewSemester(SemesterDTO semester)
        {
            if (!semesterRepo.GetAll().Any(z => z.SemesterBeginning.Year == semester.SemesterBeginning.Year && z.SemesterEnd == semester.SemesterEnd))
            {
                var newSemester = MapperFactory.CurrentMapper.Map<Semester>(semester);
                if (newSemester.SemesterBeginning.Month < 12 && newSemester.SemesterBeginning.Month >= 8)
                {
                    newSemester.SemesterName = "I. Dönem";
                    newSemester.AcademicYear = newSemester.SemesterBeginning.Year + "/" + (newSemester.SemesterBeginning.Year + 1);

                }
                else if (newSemester.SemesterBeginning.Month >= 1 && newSemester.SemesterBeginning.Month < 8)
                {
                    newSemester.SemesterName = "II. Dönem";
                    newSemester.AcademicYear = (newSemester.SemesterBeginning.Year - 1) + "/" + newSemester.SemesterBeginning.Year;
                }

                semesterRepo.Add(newSemester);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<SemesterDTO>(newSemester);
            }
            else
            {
                return null;
            }
        }

        public SemesterDTO UpdateSemester(SemesterDTO semester)
        {
            var selectedSemester = semesterRepo.Get(z => z.Id == semester.Id);
            selectedSemester = MapperFactory.CurrentMapper.Map<Semester>(semester);
            semesterRepo.Update(selectedSemester);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<SemesterDTO>(selectedSemester);

        }
    }
}
