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
    public class SemesterInformationService : ISemesterInformationService
    {
        private readonly IUnitOfWork uow;
        private IRepository<SemesterInformation> semesterRepo;

        public SemesterInformationService(IUnitOfWork _uow)
        {
            uow = _uow;
            semesterRepo = uow.GetRepository<SemesterInformation>();        
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

        public List<SemesterInformationDTO> GetAll()
        {
            var semesterList = semesterRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<SemesterInformationDTO>>(semesterList);
        }

        public SemesterInformationDTO GetSemesterInfo(int id)
        {
            var selectedSemester = semesterRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<SemesterInformationDTO>(selectedSemester);
        }

        public SemesterInformationDTO NewSemester(SemesterInformationDTO semester)
        {
            if (!semesterRepo.GetAll().Any(z => z.SemesterBeginning.Year == semester.SemesterBeginning.Year ))
            {
                var newSemester = MapperFactory.CurrentMapper.Map<SemesterInformation>(semester);
                semesterRepo.Add(newSemester);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<SemesterInformationDTO>(newSemester);
            }
            else
            {
                return null;
            }
        }

        public SemesterInformationDTO UpdateSemester(SemesterInformationDTO semester)
        {
            var selectedSemester = semesterRepo.Get(z => z.Id == semester.Id);
            selectedSemester = MapperFactory.CurrentMapper.Map<SemesterInformation>(semester);
            semesterRepo.Update(selectedSemester);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<SemesterInformationDTO>(selectedSemester);

        }
    }
}
