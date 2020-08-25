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
    public class TimetableViewService : ITimetableViewService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<TimetableView> ttViewRepo;
        private IRepository<Timetable> timetableRepo;
        private IRepository<Section> sectionRepo;
        public TimetableViewService(IUnitOfWork _uow)
        {
            uow = _uow;
            ttViewRepo = uow.GetRepository<TimetableView>();
            timetableRepo = uow.GetRepository<Timetable>();
            sectionRepo = uow.GetRepository<Section>();
        }

        public bool DeleteTimeTable(int id)
        {
            throw new NotImplementedException();
        }

        public List<TimetableViewDTO> GetAll()
        {
            var ttList = ttViewRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(ttList);
        }

        public TimetableViewDTO GetTimeTable(int id)
        {
            throw new NotImplementedException();
        }

        public List<TimetableViewDTO> GetTimetableBySection(int id)
        {
            var sectionName = sectionRepo.Get(z => z.Id == id).SectionName;
            var ttList = ttViewRepo.GetAll().Where(z => z.SectionName == sectionName);
            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(ttList);
        }

        public TimetableViewDTO NewTimeTable(TimetableViewDTO timeTable)
        {
            throw new NotImplementedException();
        }

        public TimetableViewDTO UpdateTimeTable(TimetableViewDTO timeTable)
        {
            throw new NotImplementedException();
        }
    }
}
