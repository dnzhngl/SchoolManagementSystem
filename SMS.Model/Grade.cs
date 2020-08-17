using Microsoft.Extensions.Configuration;
using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class Grade : Entity<int>
    {
        public Grade()
        {
            Sections = new HashSet<Section>();
        }
        public string GradeName { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
