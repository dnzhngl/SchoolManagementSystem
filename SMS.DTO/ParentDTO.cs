using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMS.DTO
{
    public class ParentDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [StringLength(11, ErrorMessage = "Yanlış veya eksik giriş yaptınız.")]
        public string IdentityNumber { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Address { get; set; }

        public Nullable<int> UserId { get; set; }
        public UserDTO User { get; set; }

        public List<StudentDTO> Students { get; set; }
    }
}
