using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public List<UserDTO> UserDTOs { get; set; }

    }
}
