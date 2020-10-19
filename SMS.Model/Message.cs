using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class Message : Entity<int>
    {
        [ForeignKey(nameof(Sender))]
        public Nullable<int> SenderId { get; set; }
        public virtual User Sender { get; set; }

        [ForeignKey(nameof(Receiver))]
        public Nullable<int> ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        public string Subject { get; set; }
        public string MessageContent { get; set; }
        public DateTime TimeSent { get; set; }
        public string Attachments { get; set; }
    }
}
