using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public DateTime PublishedDate { get; set; }

        public Nullable<int> UserId { get; set; }
        public UserDTO UserDTO { get; set; }

        public Nullable<int> PostCategoryId { get; set; }
        public PostCategoryDTO PostCategoryDTO { get; set; }
    }
}
