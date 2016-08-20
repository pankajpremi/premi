using JMM.APEC.Common;
using JMM.APEC.Common.Interfaces;
using JMM.APEC.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JMM.APEC.ActionService
{
    public class MessageService : IMessageService
    {
        private int? portalId = null;
        private int? gatewayId = null;
        private string provider = string.Empty;
        private string templateRoot = string.Empty;
        private ApecDatabase database = null;
        private IDaoFactory factory = null;

        private IEmailTemplateDao EmailTemplateDao;

        public MessageService(int? PortalId, int? GatewayId)
        {
            this.portalId = PortalId;
            this.gatewayId = GatewayId;
            templateRoot = ConfigurationManager.AppSettings["EmailTemplateRoot"].ToString();

            provider = "ApecDataProvider";
            database = new ApecDatabase(provider, null);
            factory = database.GetFactory();

            EmailTemplateDao = factory.EmailTemplateDao;
        }

        private void SendEmailMessage(string EmailAddress, string FromAddress, string Subject, string Body, string SmtpServer, string Username, string Password, bool EnableSsl, int Port)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            string[] emails = EmailAddress.Split(new Char[] { ',', ';' });
            foreach (var email in emails)
            {
                message.To.Add(email);
            }

            message.From = new System.Net.Mail.MailAddress(FromAddress);
            message.IsBodyHtml = true;
            message.Subject = Subject;
            message.Body = Body;

            System.Net.Mail.SmtpClient server = new System.Net.Mail.SmtpClient(SmtpServer);
            server.UseDefaultCredentials = false;
            server.Credentials = new System.Net.NetworkCredential(Username, Password);
            server.EnableSsl = EnableSsl;
            server.Port = Port;
            server.Send(message);
        }

        public void SendEmail(string ToAddress, string FromAddress, string Subject, string Body)
        {
            var code = "";

            var transport = EmailTemplateDao.GetTransport(code).FirstOrDefault();

            if (transport != null)
            {
                try
                {
                    SendEmailMessage(ToAddress, FromAddress, Subject, Body, transport.SmtpServer, transport.Username, transport.Password, transport.EnableSsl, transport.Port);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return;
                }
            }
        }

        public void SendEmail(string TemplateCode, string EmailAddress, object Message)
        {
            var template = EmailTemplateDao.GetTemplate(TemplateCode, portalId, gatewayId).FirstOrDefault();

            if (template != null)
            {
                try
                {
                    var emailContainer = File.ReadAllText(templateRoot + template.ContainerUrl);
                    var emailTemplate = File.ReadAllText(templateRoot + template.TemplateUrl);
                    
                    var container = MergeMessage(emailContainer, Message);
                    var tempbody = new EmailMessageObject();
                    tempbody.Content = MergeMessage(emailTemplate, Message);

                    var subject = MergeMessage(template.Subject, Message);
                    var body = MergeMessage(container, tempbody);

                    SendEmailMessage(EmailAddress, template.FromAddress, subject, body, template.SmtpServer, template.Username, template.Password, template.EnableSsl, template.Port);

                    
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return;
                }
            }
        }

        public async Task SendEmailAsync(string TemplateCode, string EmailAddress, object Message)
        {
            var template = EmailTemplateDao.GetTemplate(TemplateCode, portalId, gatewayId).FirstOrDefault();

            if (template != null)
            {

                try
                {
                    var emailContainer = File.ReadAllText(templateRoot + template.ContainerUrl);
                    var emailTemplate = File.ReadAllText(templateRoot + template.TemplateUrl);

                    var container = MergeMessage(emailContainer, Message);
                    var tempbody = new EmailMessageObject();
                    tempbody.Content = MergeMessage(emailTemplate, Message);

                    var subject = MergeMessage(template.Subject, Message);
                    var body = MergeMessage(container, tempbody);

                    SendEmailMessage(EmailAddress, template.FromAddress, subject, body, template.SmtpServer, template.Username, template.Password, template.EnableSsl, template.Port);

                    await Task.Yield();

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return;
                }
            }
        }

        public string MergeMessage(string template, object data)
        {
            var props = data.GetType().GetProperties();

            foreach (var prop in props)
            {
                var pattern = new Regex("%" + prop.Name + "%");
                template = pattern.Replace(template, String.Format("{0}", prop.GetValue(data, null)));
            }

            return template;
        }

        public async Task NotifyEmailAdmins(string NotificationCode, object Data)
        {
            var recipients = string.Empty;

            recipients = ConfigurationManager.AppSettings["RequestAccessEmailList"].ToString();

            SendEmail(NotificationCode, recipients, Data);

            await Task.Yield();
        }
    }

    public class EmailMessageObject
    {
        public string Content { get; set; }
    }
}
