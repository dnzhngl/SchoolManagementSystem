using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string IdentityNumber { get; set; }
        public string SchoolNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }

        public string GraduatedFrom { get; set; } //Mezun olduğu okul
        public decimal GPA { get; set; }    //Not ortalaması
        public bool StudentStatusBool { get; set; }    //İlişkisi Devam Ediyor, İlişkisi Kesildi (Öğrencilik Durumu)
        public string StudentStatus { get; set; }  


        public Nullable<int> SectionId { get; set; }
        public SectionDTO Section { get; set; }

        public int ParentId { get; set; }
        public ParentDTO Parent { get; set; }

        public Nullable<int> UserId { get; set; }
        public UserDTO User { get; set; }

        public List<AttendanceDTO> Attendances { get; set; }
        public List<ExamResultDTO> ExamResults { get; set; }
        public List<CertificateDTO> Certificates { get; set; }
    }
}
