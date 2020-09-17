using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class CertificateType : Entity<int>
    {
        public CertificateType()
        {
            Certificates = new HashSet<Certificate>();
        }
        public string CertificateTypeName { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}
