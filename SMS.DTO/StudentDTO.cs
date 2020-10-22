using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMS.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [StringLength(11, ErrorMessage = "Yanlış veya eksik giriş yaptınız.")]
        public string IdentityNumber { get; set; }
        public string SchoolNumber { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string GraduatedFrom { get; set; } //Mezun olduğu okul
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public decimal GPA { get; set; }    //Not ortalaması
        public bool StudentStatusBool { get; set; }    //İlişkisi Devam Ediyor, İlişkisi Kesildi (Öğrencilik Durumu)
        public string StudentStatus { get; set; }
        public DateTime RegistrationDate { get; set; }


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
