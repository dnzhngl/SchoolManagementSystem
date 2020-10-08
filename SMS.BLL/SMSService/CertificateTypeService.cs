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
    public class CertificateTypeService : ICertificateTypeService
    {
        private readonly IUnitOfWork uow;
        private IRepository<CertificateType> certificateTypeRepo;

        public CertificateTypeService(IUnitOfWork _uow)
        {
            uow = _uow;
            certificateTypeRepo = uow.GetRepository<CertificateType>();
        }
        public bool DeleteCertificateType(int certificateTypeId)
        {
            try
            {
                var selectedCertificateType = certificateTypeRepo.Get(z => z.Id == certificateTypeId);
                certificateTypeRepo.Delete(selectedCertificateType);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<CertificateTypeDTO> GetAll()
        {
            var certificateTypeList = certificateTypeRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<CertificateTypeDTO>>(certificateTypeList);
        }

        public CertificateTypeDTO GetCertificateType(int certificateTypeId)
        {
            var selectedCertificateType = certificateTypeRepo.Get(z => z.Id == certificateTypeId);
            return MapperFactory.CurrentMapper.Map<CertificateTypeDTO>(selectedCertificateType);
        }

        public CertificateTypeDTO GetCertificateType(string certificateTypeName)
        {
            var selectedCertificateType = certificateTypeRepo.Get(z => z.CertificateTypeName.Contains(certificateTypeName));
            return MapperFactory.CurrentMapper.Map<CertificateTypeDTO>(selectedCertificateType);
        }

        public CertificateTypeDTO NewCertificateType(CertificateTypeDTO certificateType)
        {
            if (!certificateTypeRepo.GetAll().Any(z => z.CertificateTypeName == certificateType.CertificateTypeName))
            {
                var newCertificateType = MapperFactory.CurrentMapper.Map<CertificateType>(certificateType);
                certificateTypeRepo.Add(newCertificateType);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<CertificateTypeDTO>(newCertificateType);
            }
            else
            {
                return null;
            }
        }

        public CertificateTypeDTO UpdateCertificateType(CertificateTypeDTO certificateType)
        {
            var selectedCertificateType = certificateTypeRepo.Get(z => z.Id == certificateType.Id);
            selectedCertificateType = MapperFactory.CurrentMapper.Map<CertificateType>(certificateType);
            certificateTypeRepo.Update(selectedCertificateType);
            uow.SaveChanges();

            return MapperFactory.CurrentMapper.Map<CertificateTypeDTO>(selectedCertificateType);
        }
    }
}
