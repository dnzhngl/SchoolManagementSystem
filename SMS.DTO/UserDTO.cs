using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LoginTimeStamps { get; set; }
        public DateTime? FailedLogin { get; set; }

        public Nullable<int> RoleId { get; set; }
        public RoleDTO Role { get; set; }

        public List<PostDTO> Posts { get; set; }
        public List<MessageDTO> MessageSender { get; set; }
        public List<MessageDTO> MessageReceiver { get; set; }
    }
}
