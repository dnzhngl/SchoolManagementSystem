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
    public class DayService : IDayService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Day> dayRepo;
        public DayService(IUnitOfWork _uow)
        {
            uow = _uow;
            dayRepo = uow.GetRepository<Day>();
        }
        public List<DayDTO> GetAll()
        {
            var days = dayRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<DayDTO>>(days);
        }

        public DayDTO GetDay(int id)
        {
            var selectedDay = dayRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<DayDTO>(selectedDay);

        }

        public DayDTO GetDayByName(string dayName)
        {
            var day = dayRepo.Get(z => z.DayName == dayName);
            return MapperFactory.CurrentMapper.Map<DayDTO>(day);
        }
    }
}
