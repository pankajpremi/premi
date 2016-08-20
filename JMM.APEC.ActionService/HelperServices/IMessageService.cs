using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActionService
{
    public interface IMessageService
    {
        //a new service for sending messages
        void SendEmail(string TemplateCode, string EmailAddress, object Message);
        Task SendEmailAsync(string TemplateCode, string EmailAddress, object Message);
        Task NotifyEmailAdmins(string NotificationCode, object Data);
    }
}
