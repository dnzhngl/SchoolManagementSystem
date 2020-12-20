using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ICertificateService : IServiceBase
    {
        List<CertificateDTO> GetAll();
        CertificateDTO GetCertificate(int certificateId);
        List<CertificateDTO> GetCertificateList(int studentId);
        CertificateDTO NewCertificate(CertificateDTO certificate);
        CertificateDTO UpdateCertificate(CertificateDTO certificate);
        bool DeleteCertificate(int certificateId);
        List<CertificateDTO> GetAllCertificatesInclude();
        CertificateDTO CreateCertificate(int studentId, int semesterId);

        List<CertificateDTO> GetCertificateList(int studentId, int semesterId);
    }
}
