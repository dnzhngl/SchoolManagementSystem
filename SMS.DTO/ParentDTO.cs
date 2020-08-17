﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class ParentDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Address { get; set; }

        public List<StudentDTO> Students { get; set; }
    }
}