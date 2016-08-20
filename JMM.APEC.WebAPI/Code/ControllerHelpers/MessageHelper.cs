using JMM.APEC.Core;
using JMM.APEC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Code
{
    public class MessageHelper
    {

        public static SystemMessageDto MakeMessageDto(System_Message message)
        {
            var rMessage = new SystemMessageDto
            {
                MessageId = message.MessageId,
                FromUserId = message.UserFromId,
                ToUserId = (int)message.UserToId,
                BeginDateTime = message.BeginDateTime,
                Subject = message.Subject,
                Message = message.Message
            };

            return rMessage;
        }

    }
}