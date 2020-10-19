using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;

namespace SMS.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService messageService;
        private readonly IUserService userService;
        public MessageController(IMessageService _messageService, IUserService _userService)
        {
            messageService = _messageService;
            userService = _userService;
        }
        public IActionResult MessageList(string username)
        {
            var user = userService.GetUserByUsername(username);
            var messageList = messageService.GetAllMessages(user.Id);
            return View(messageList);
        }
        public IActionResult Inbox(string username)
        {
            var user = userService.GetUserByUsername(username);
            var messageList = messageService.IncomingMessages(user.Id);
            return View(messageList);
        }
        public IActionResult Outbox(string username)
        {
            var user = userService.GetUserByUsername(username);
            var messageList = messageService.SentMessages(user.Id);
            return View(messageList);
        }
        public IActionResult DeleteMessage(int messageId)
        {
            messageService.DeleteMessage(messageId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet]
        public IActionResult SendMessage(string sender, string receiver)
        {
            var message = new MessageDTO();
            message.Sender = userService.GetUserByUsername(sender);
            message.SenderId = message.Sender.Id;
            message.Receiver = userService.GetUserByUsername(receiver);
            message.ReceiverId = message.Receiver.Id;

            return View(message);
        }
        [HttpPost]
        public IActionResult SendMessage(MessageDTO message)
        {
            MessageDTO newMsg = new MessageDTO();
            newMsg.Subject = message.Subject;
            newMsg.MessageContent = message.MessageContent;
            newMsg.ReceiverId = message.ReceiverId;
            newMsg.SenderId = message.SenderId;
            newMsg.TimeSent = DateTime.Now;
            newMsg.Attachments = message.Attachments;
            //message.Receiver = userService.GetUser((int)message.ReceiverId);
            //message.Sender = userService.GetUser((int)message.SenderId);
            messageService.NewMessage(newMsg);

            return RedirectToAction("MessageList", new {username = this.User.Identity.Name });
        }

        public IActionResult ShowMessage(int messageId)
        {
            MessageDTO message = messageService.GetMessage(messageId);
            return View(message);
        }
    }
}
