using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class PostCategoryDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public List<PostDTO> Posts { get; set; }
    }
}
