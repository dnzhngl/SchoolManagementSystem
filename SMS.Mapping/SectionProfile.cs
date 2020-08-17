using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Mapping
{
    public class SectionProfile : ProfileBase
    {
        public SectionProfile()
        {
            CreateMap<Section, SectionDTO>().ReverseMap();
        }
    }
}
