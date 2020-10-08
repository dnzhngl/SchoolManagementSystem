using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Student : Entity<int>
    {
        public Student()
        {
            Attendances = new HashSet<Attendance>();
            ExamResults = new HashSet<ExamResult>();
            Certificates = new HashSet<Certificate>();
        }
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


        [ForeignKey("Section")]
        public Nullable<int> SectionId { get; set; }
        [DefaultValue("Not Assigned")]
        public virtual Section Section { get; set; }


        [ForeignKey("Parent")]
        public int ParentId { get; set; }
        public virtual Parent Parent { get; set; }


        [ForeignKey("User")]
        public Nullable<int> UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}
