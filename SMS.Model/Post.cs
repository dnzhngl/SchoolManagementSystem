using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Post : Entity<int>
    {
        public string PostName { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PostContent { get; set; }
        public string File { get; set; }

        [ForeignKey("User")]
        public Nullable<int> UserId { get; set; }
        public virtual User User { get; set; }

        
        [ForeignKey("PostCategory")]
        public Nullable<int> PostCategoryId { get; set; }
        public virtual PostCategory PostCategory { get; set; }
    }
}
