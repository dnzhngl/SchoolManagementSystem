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
    public class ExamTypeService : IExamTypeService
    {
        private readonly IUnitOfWork uow;
        private IRepository<ExamType> examTypeRepo;
        public ExamTypeService(IUnitOfWork _uow)
        {
            uow = _uow;
            examTypeRepo = uow.GetRepository<ExamType>();
        }
        public List<ExamTypeDTO> GetAll()
        {
            var examTypeList = examTypeRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<ExamTypeDTO>>(examTypeList);
        }

        public ExamTypeDTO GetExamType(int id)
        {
            var selectedExamType = examTypeRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<ExamTypeDTO>(selectedExamType);
        }
    }
}
