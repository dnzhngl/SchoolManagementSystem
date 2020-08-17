using SMS.BLL.Abstract;
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
    public class ParentService : IParentService
    {
        private readonly IUnitOfWork uow;
        public ParentService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public bool DeleteParent(int id)
        {
            try
            {
                var selectedParent = uow.GetRepository<Parent>().Get(z => z.Id == id);
                uow.GetRepository<Parent>().Delete(selectedParent);
                uow.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ParentDTO> GetAll()
        {
            var parentList = uow.GetRepository<Parent>().GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<ParentDTO>>(parentList);
        }

        public ParentDTO GetParent(int id)
        {
            var selectedParent = uow.GetRepository<Parent>().Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<ParentDTO>(selectedParent);
        }

        public List<ParentDTO> GetParentName(string name)
        {
            throw new NotImplementedException();
        }

        public ParentDTO NewParent(ParentDTO parent)
        {
            var newParent = MapperFactory.CurrentMapper.Map<Parent>(parent);
            uow.GetRepository<Parent>().Add(newParent);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<ParentDTO>(newParent);
        }

        public ParentDTO UpdateParent(ParentDTO parent)
        {
            var selectedParent = uow.GetRepository<Parent>().Get(z => z.Id == parent.Id);
            selectedParent = MapperFactory.CurrentMapper.Map<Parent>(parent);
            uow.GetRepository<Parent>().Update(selectedParent);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<ParentDTO>(selectedParent);
        }
    }
}
