using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ICertificateTypeService : IServiceBase
    {
        List<CertificateTypeDTO> GetAll();
        CertificateTypeDTO GetCertificateType(int certificateTypeId);
        CertificateTypeDTO GetCertificateType(string certificateTypeName);
        CertificateTypeDTO NewCertificateType(CertificateTypeDTO certificateType);
        CertificateTypeDTO UpdateCertificateType(CertificateTypeDTO certificateType);
        bool DeleteCertificateType(int certificateTypeId);
    }
}
