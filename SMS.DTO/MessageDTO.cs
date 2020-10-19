using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public Nullable<int> SenderId { get; set; }
        public UserDTO Sender { get; set; }
        public Nullable<int> ReceiverId { get; set; }
        public UserDTO Receiver { get; set; }

        public string Subject { get; set; }
        public string MessageContent { get; set; }
        public DateTime TimeSent { get; set; }
        public string Attachments { get; set; }
    }
}
