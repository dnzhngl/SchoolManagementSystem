using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class Day : Entity<int>
    {
        public Day()
        {
            TimeTables = new HashSet<TimeTable>();
        }
        public string DayName { get; set; }
        public ICollection<TimeTable> TimeTables { get; set; }
    }
}
