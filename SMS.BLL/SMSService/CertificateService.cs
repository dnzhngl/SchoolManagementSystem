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
    public class CertificateService : ICertificateService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Certificate> certificateRepo;
        public CertificateService(IUnitOfWork _uow)
        {
            uow = _uow;
            certificateRepo = uow.GetRepository<Certificate>();
        }
        public bool DeleteCertificate(int certificateId)
        {
            try
            {
                var selectedCertificate = certificateRepo.Get(z => z.Id == certificateId);
                certificateRepo.Delete(selectedCertificate);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<CertificateDTO> GetAll()
        {
            var certificateList = certificateRepo.GetIncludesList(null, z => z.Semester).ToList();
            return MapperFactory.CurrentMapper.Map<List<CertificateDTO>>(certificateList);
        }

        public CertificateDTO GetCertificate(int certificateId)
        {
            var selectedCertificate = certificateRepo.GetIncludes(z => z.Id == certificateId, z => z.Semester);
            return MapperFactory.CurrentMapper.Map<CertificateDTO>(selectedCertificate);
        }

        /// <summary>
        /// It gives certificate list of specified student with Certificate type
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>List of certificates</returns>
        public List<CertificateDTO> GetCertificateList(int studentId)
        {
            //var studentsCertificates = certificateRepo.GetAll().Where(z => z.StudentId == studentId).ToList();
            var studentsCertificates = certificateRepo.GetIncludesList(z => z.StudentId == studentId, z => z.CertificateType, z => z.Semester).ToList();
            return MapperFactory.CurrentMapper.Map<List<CertificateDTO>>(studentsCertificates);
        }

        public CertificateDTO NewCertificate(CertificateDTO certificate)
        {
            if (!certificateRepo.GetAll().Any(z => z.CertificateTypeId == certificate.CertificateTypeId && z.SemesterId == certificate.SemesterId && z.StudentId == certificate.StudentId))
            {
                var newCertificate = MapperFactory.CurrentMapper.Map<Certificate>(certificate);
               // newCertificate.SerialNumber = GenerateSerialNumber();
                certificateRepo.Add(newCertificate);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<CertificateDTO>(newCertificate);
            }
            else
            {
                return null;
            }
        }

        public CertificateDTO UpdateCertificate(CertificateDTO certificate)
        {
            var selectedCertificate = certificateRepo.Get(z => z.Id == certificate.Id);
            selectedCertificate = MapperFactory.CurrentMapper.Map<Certificate>(certificate);
            certificateRepo.Update(selectedCertificate);
            uow.SaveChanges();

            return MapperFactory.CurrentMapper.Map<CertificateDTO>(selectedCertificate);
        }
        public List<CertificateDTO> GetAllCertificatesInclude()
        {
            var certificateList = certificateRepo.Get(z => z.Student, z => z.CertificateType, z => z.Semester).ToList();
            return MapperFactory.CurrentMapper.Map<List<CertificateDTO>>(certificateList);
        }

        Guid GenerateSerialNumber()
        {
            Guid serialNumber = Guid.NewGuid();
            return serialNumber;
        }
    }
}
