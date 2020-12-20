using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class DismissedStudentView
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
        public string StudentStatus { get; set; }
        public DateTime? StudentStatusEditDate { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Nullable<int> SectionId { get; set; }
        public int ParentId { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
