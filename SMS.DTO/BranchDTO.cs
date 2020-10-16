using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMS.DTO
{
    public class BranchDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string BranchName { get; set; }
        public List<InstructorDTO> InstructorDTOs { get; set; }
    }
}
