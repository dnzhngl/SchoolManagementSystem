using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Mapping
{
    public class PostProfile : ProfileBase
    {
        public PostProfile()
        {
            CreateMap<Post, PostDTO>().ReverseMap();
        }
    }
}
