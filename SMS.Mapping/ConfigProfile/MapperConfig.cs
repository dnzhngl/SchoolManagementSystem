﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Mapping.ConfigProfile
{
    public class MapperConfig
    {
        public static void RegisterMappers()
        {
            MapperFactory.RegisterMappers();
        }
    }
}
