using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMS.DTO;


namespace SMS.WebUI.Models
{
    public class MainPageViewModel
    {
        public List<PostDTO> PostDTOs { get; set; }
        public UserDTO UserDTO { get; set; }
    }
}
