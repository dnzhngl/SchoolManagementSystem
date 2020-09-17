﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime LoginTimeStamps { get; set; }
        public DateTime FailedLogin { get; set; }

        public Nullable<int> RoleId { get; set; }
        public RoleDTO RoleDTO { get; set; }

        public List<PostDTO> PostDTOs { get; set; }
    }
}
