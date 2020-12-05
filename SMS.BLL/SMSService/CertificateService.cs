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
        private IRepository<CertificateType> certificateTypeRepo;
        private IRepository<StudentSchoolReportView> schoolReportRepo;
        public CertificateService(IUnitOfWork _uow)
        {
            uow = _uow;
            certificateRepo = uow.GetRepository<Certificate>();
            certificateTypeRepo = uow.GetRepository<CertificateType>();
            schoolReportRepo = uow.GetRepository<StudentSchoolReportView>();
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
            var certificateList = certificateRepo.GetIncludesList(null, z => z.CertificateType, z => z.Semester).ToList();
            return MapperFactory.CurrentMapper.Map<List<CertificateDTO>>(certificateList);
        }

        public CertificateDTO GetCertificate(int certificateId)
        {
            var selectedCertificate = certificateRepo.GetIncludes(z => z.Id == certificateId, z => z.Semester, z => z.CertificateType);
            return MapperFactory.CurrentMapper.Map<CertificateDTO>(selectedCertificate);
        }

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

        public CertificateDTO CreateCertificate(int studentId, int semesterId)
        {
            var yearEndMarks = schoolReportRepo.GetAll().Where(z => z.Id == studentId).GroupBy(z => new { z.Id, z.SchoolNumber, z.FirstName, z.LastName, z.AcademicYear }).Select(x => new
            {
                x.Key.Id,
                x.Key.SchoolNumber,
                x.Key.FirstName,
                x.Key.LastName,
                GPA = x.Sum(z => z.GPA),
                GPANumeral = x.Sum(z => z.GpaNumeral),
                WeeklyCourseHours = x.Sum(z => z.WeeklyCourseHours),
                x.Key.AcademicYear
            });

            Certificate certificate = new Certificate();
            certificate.CertificateType = new CertificateType();

            var result = yearEndMarks.First();
            if ((result.GPA/result.WeeklyCourseHours) >= 70 & (result.GPA/result.WeeklyCourseHours) < 85)
            {
                certificate.StudentId = studentId;
                certificate.SemesterId = semesterId;
                certificate.CertificateTypeId = certificateTypeRepo.Get(z => z.CertificateTypeName.Contains("Teşekkür Belgesi")).Id;
                certificate.CertificateType.CertificateTypeName = certificateTypeRepo.Get(z => z.Id == certificate.CertificateTypeId).CertificateTypeName;
                certificate.IssueDate =  DateTime.Today;
                             }
            else if (result.GPA >= 85)
            {
                certificate.StudentId = studentId;
                certificate.SemesterId = semesterId;
                certificate.CertificateTypeId = certificateTypeRepo.Get(z => z.CertificateTypeName.Contains("Takdir Belgesi")).Id;
                certificate.CertificateType.CertificateTypeName = certificateTypeRepo.Get(z => z.Id == certificate.CertificateTypeId).CertificateTypeName;
                certificate.IssueDate = DateTime.Today;
            }
            return MapperFactory.CurrentMapper.Map<CertificateDTO>(certificate);
        }

        Guid GenerateSerialNumber()
        {
            Guid serialNumber = Guid.NewGuid();
            return serialNumber;
        }
    }
}
