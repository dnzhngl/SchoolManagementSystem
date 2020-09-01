using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class SectionService : ISectionService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Section> sectionRepo;
        public SectionService(IUnitOfWork _uow)
        {
            uow = _uow;
            sectionRepo = uow.GetRepository<Section>();
        }

        public bool DeleteSection(int id)
        {
            try
            {
                var selectedSection = sectionRepo.Get(z => z.Id ==
                id);
                sectionRepo.Delete(selectedSection);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SectionDTO> GetAll()
        {
            var sectionList = sectionRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<SectionDTO>>(sectionList);
        }

        public SectionDTO GetSection(int id)
        {
            var selectedSection = sectionRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<SectionDTO>(selectedSection);
        }
        public List<SectionDTO> GetSectionByGrade(int gradeId)
        {
            //var test = sectionRepo.Get(null, x => x.Grade).ToList();// --Selimden

            var sectionList = sectionRepo.GetAll().Where(z => z.GradeId == gradeId);
            //var sectionList = sectionRepo.Get(null, x => x.Grade).ToList();
            return MapperFactory.CurrentMapper.Map<List<SectionDTO>>(sectionList);
        }

        public SectionDTO GetSectionByName(string sectionName)
        {
            var section = sectionRepo.Get(z => z.SectionName == sectionName);
            return MapperFactory.CurrentMapper.Map<SectionDTO>(section);
        }

        public SectionDTO NewSection(SectionDTO section)
        {
            if (!sectionRepo.GetAll().Any(z => z.SectionName.ToLower() == section.SectionName.ToLower()))
            {
                var newSection = MapperFactory.CurrentMapper.Map<Section>(section);
                newSection.GradeId = section.GradeId;
                newSection = sectionRepo.Add(newSection);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<SectionDTO>(newSection);
            }
            else
            {
                return null;
            }

        }

        public SectionDTO UpdateSection(SectionDTO section)
        {
            var selectedSection = sectionRepo.Get(z => z.Id == section.Id);
            selectedSection = MapperFactory.CurrentMapper.Map<Section>(section);
            sectionRepo.Update(selectedSection);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<SectionDTO>(selectedSection);
        }
    }
}
