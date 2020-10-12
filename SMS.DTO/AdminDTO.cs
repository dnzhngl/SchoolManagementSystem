using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class AdminDTO
    {
        public int Id { get; set; }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string Duty { get; set; }

        public Nullable<int> UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
