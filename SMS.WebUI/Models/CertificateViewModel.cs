using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class CertificateViewModel
    {
        public CertificateDTO CertificateDTO { get; set; }
        public List<CertificateDTO> CertificateDTOs { get; set; }

        public List<CertificateTypeDTO> CertificateTypeDTOs { get; set; }

        public StudentDTO StudentDTO { get; set; }
        public List<StudentDTO> StudentDTOs { get; set; }

        public List<SemesterDTO> SemesterDTOs { get; set; }
        public SemesterDTO SemesterDTO { get; set; }
    }
}
