using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Mapping
{
    public class AdminProfile : ProfileBase
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminDTO>().ReverseMap();
        }
    }
}
