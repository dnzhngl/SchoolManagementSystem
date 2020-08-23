using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Mapping
{
    public class SemesterProfile : ProfileBase
    {
        public SemesterProfile()
        {
            CreateMap<Semester, SemesterDTO>().ReverseMap();
        }
    }
}
