﻿using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Mapping
{
    public class ExamProfile : ProfileBase
    {
        public ExamProfile()
        {
            CreateMap<Exam, ExamDTO>().ReverseMap();
        }
    }
}
