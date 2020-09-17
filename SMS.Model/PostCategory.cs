using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Model
{
    public class PostCategory : Entity<int>
    {
        public PostCategory()
        {
            Posts = new HashSet<Post>();
        }
        public string CategoryName { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

    }
}
