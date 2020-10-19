using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IMessageService : IServiceBase
    {
        List<MessageDTO> GetAllMessages(int userId);
        List<MessageDTO> SentMessages(int userId);
        List<MessageDTO> IncomingMessages(int userId);
        MessageDTO GetMessage(int messageId);
        MessageDTO NewMessage(MessageDTO newMessage);
        bool DeleteMessage(int messageId);
    }
}
