using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Grade> gradeRepo;
        public GradeService(IUnitOfWork _uow)
        {
            uow = _uow;
            gradeRepo = uow.GetRepository<Grade>();
        }

        public bool DeleteGrade(int id)
        {
            try
            {
                var selectedGrade = gradeRepo.Get(z => z.Id == id);
                gradeRepo.Delete(selectedGrade);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }  
        }

        public List<GradeDTO> GetAll()
        {
            var gradeList = gradeRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<GradeDTO>>(gradeList);
        }

        public GradeDTO GetGrade(int id)
        {
            var selectedGrade = gradeRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<GradeDTO>(selectedGrade);
        }

        public GradeDTO NewGrade(GradeDTO grade)
        {
            if (!gradeRepo.GetAll().Any(z => z.GradeName.ToLower() == grade.GradeName.ToLower()))
            {
                var newGrade = MapperFactory.CurrentMapper.Map<Grade>(grade);
                newGrade = gradeRepo.Add(newGrade);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<GradeDTO>(newGrade);
            }
            else
            {
                return null;
            }

        }

        public GradeDTO UpdateGrade(GradeDTO grade)
        {
            var selectedGrade = gradeRepo.Get(z => z.Id == grade.Id);
            selectedGrade = MapperFactory.CurrentMapper.Map<Grade>(grade);
            gradeRepo.Update(selectedGrade);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<GradeDTO>(selectedGrade);
        }
    }
}
