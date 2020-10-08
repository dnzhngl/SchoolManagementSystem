using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class CertificateTypeDTO
    {
        public int Id { get; set; }
        public string CertificateTypeName { get; set; }
        public List<CertificateDTO> Certificates { get; set; }
    }
}
