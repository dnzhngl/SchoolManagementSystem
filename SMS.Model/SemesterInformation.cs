using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class SemesterInformation : Entity<int>
    {
        public DateTime SemesterBeginning { get; set; }
        public DateTime SemesterEnd { get; set; }

    }
}
