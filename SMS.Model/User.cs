using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMS.Model
{
    public class User : Entity<int>
    {
        public User()
        {
            Posts = new HashSet<Post>();
            MessageReceiver = new HashSet<Message>();
            MessageSender = new HashSet<Message>();
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LoginTimeStamps { get; set; }
        public DateTime? FailedLogin { get; set; }


        [ForeignKey("Role")]
        public Nullable<int> RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        [InverseProperty(nameof(Message.Sender))]
        public ICollection<Message> MessageSender { get; set; }

        [InverseProperty(nameof(Message.Receiver))]
        public ICollection<Message> MessageReceiver { get; set; }
    }
}
