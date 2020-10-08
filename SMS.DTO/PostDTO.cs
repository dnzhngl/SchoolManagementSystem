using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PostContent { get; set; }
        public string File { get; set; }

        public Nullable<int> UserId { get; set; }
        public UserDTO User { get; set; }

        public Nullable<int> PostCategoryId { get; set; }
        public PostCategoryDTO PostCategory { get; set; }
    }
}
