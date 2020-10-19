using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Message> messageRepo;
        public MessageService(IUnitOfWork _uow)
        {
            uow = _uow;
            messageRepo = uow.GetRepository<Message>();
        }
        public bool DeleteMessage(int messageId)
        {
            try
            {
                var selectedMessage = messageRepo.Get(z => z.Id == messageId);
                messageRepo.Delete(selectedMessage);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public MessageDTO GetMessage(int messageId)
        {
            //var selectedMessage = messageRepo.Get(z => z.Id == messageId);
            var selectedMessage = messageRepo.GetIncludes(z => z.Id == messageId, z => z.Receiver, z => z.Sender);
            return MapperFactory.CurrentMapper.Map<MessageDTO>(selectedMessage);
        }
        public List<MessageDTO> GetAllMessages(int userId)
        {
            // var messageList = messageRepo.GetAll().ToList();
            var messageList = messageRepo.GetIncludesList(z=>z.SenderId == userId || z.ReceiverId == userId, z => z.Receiver, z => z.Sender).OrderBy(z => z.TimeSent).ToList();
            return MapperFactory.CurrentMapper.Map<List<MessageDTO>>(messageList);
        }
        public List<MessageDTO> SentMessages(int userId)
        {
            //var messageList = messageRepo.GetAll().Where(z => z.SenderId == userId).ToList();
            var messageList = messageRepo.GetIncludesList(z => z.SenderId == userId, z => z.Receiver).ToList();
            return MapperFactory.CurrentMapper.Map<List<MessageDTO>>(messageList);
        }
        public List<MessageDTO> IncomingMessages(int userId)
        {
            // var messageList = messageRepo.GetAll().Where(z => z.ReceiverId == userId).ToList();
            var messageList = messageRepo.GetIncludesList(z => z.ReceiverId == userId, z => z.Sender).ToList();
            return MapperFactory.CurrentMapper.Map<List<MessageDTO>>(messageList);
        }

        public MessageDTO NewMessage(MessageDTO newMessage)
        {
            var message = MapperFactory.CurrentMapper.Map<Message>(newMessage);
            messageRepo.Add(message);
            uow.SaveChanges();

            return MapperFactory.CurrentMapper.Map<MessageDTO>(message);
        }
    }
}
