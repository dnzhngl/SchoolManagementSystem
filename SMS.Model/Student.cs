using SMS.Core.Entities;
using System;
using System.Collections.Generic;
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
        }

        //public string SchoolNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }

        public string GraduatedFrom { get; set; } //Mezun olduğu okul
        public decimal GPA { get; set; }    //Not ortalaması


        [ForeignKey("Section")]
        public Nullable<int> SectionId { get; set; }
        public Section Section { get; set; }


        [ForeignKey("Parent")]
        [Required(ErrorMessage = "Veli bilgisi boş geçilemez.")]
        public int ParentId { get; set; } 
        public Parent Parent { get; set; }


        public ICollection<Attendance> Attendances { get; set; }
    }
}
