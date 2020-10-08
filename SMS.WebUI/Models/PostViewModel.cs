using Microsoft.AspNetCore.Http;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Models
{
    public class PostViewModel
    {
        public PostDTO PostDTO { get; set; }
        public List<PostDTO> PostDTOs { get; set; }
        public IFormFile File { get; set; }
    }
}
