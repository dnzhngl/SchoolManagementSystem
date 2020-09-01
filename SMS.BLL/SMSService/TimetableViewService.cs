﻿using SMS.BLL.Abstract;
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
            try
            {
                var selectedTt = ttViewRepo.Get(z => z.Id == id);
                ttViewRepo.Delete(selectedTt);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<TimetableViewDTO> GetAll()
        {
            var ttList = ttViewRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(ttList);
        }

        public TimetableViewDTO GetTimeTable(int id)
        {
            var selectedTt = ttViewRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<TimetableViewDTO>(selectedTt);

        }

        public List<TimetableViewDTO> GetTimetableBySection(int id)
        {
            var sectionName = sectionRepo.Get(z => z.Id == id).SectionName;
            var ttList = ttViewRepo.GetAll().Where(z => z.SectionName == sectionName);
            return MapperFactory.CurrentMapper.Map<List<TimetableViewDTO>>(ttList);
        }

        public TimetableViewDTO NewTimeTable(TimetableViewDTO timetable)
        {
            if (!ttViewRepo.GetAll().Any(z => z.Instructor == timetable.Instructor && z.LessonPeriod == timetable.LessonPeriod && z.DayName == timetable.DayName))
            {
                TimetableView newTt = MapperFactory.CurrentMapper.Map<TimetableView>(timetable);
                ttViewRepo.Add(newTt);
                uow.SaveChanges();

                return MapperFactory.CurrentMapper.Map<TimetableViewDTO>(newTt);
            }
            else
            {
                return null;
            }
        }

        public TimetableViewDTO UpdateTimeTable(TimetableViewDTO timeTable)
        {
            var selectedTimetable = ttViewRepo.Get(z => z.Id == timeTable.Id);
            selectedTimetable = MapperFactory.CurrentMapper.Map<TimetableView>(timeTable);
            ttViewRepo.Update(selectedTimetable);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<TimetableViewDTO>(selectedTimetable);
        }
    }
}
