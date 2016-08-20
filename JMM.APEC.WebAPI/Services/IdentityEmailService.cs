using JMM.APEC.ActionService;
using JMM.APEC.Core;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Infrastructure;
using JMM.APEC.WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace JMM.APEC.WebAPI.Services
{
    public class IdentityEmailService : IIdentityMessageService
    {
        private IMessageService messageService;

        public async Task SendAsync(IdentityMessage message)
        {
            await configSendEmailAsync(message);
        }

        private async Task configSendEmailAsync(IdentityMessage message)
        {
            IdentityDatabase database = new IdentityDatabase();

            //get user info
            UserManager<ApplicationIdentityUser, int> manager = new UserManager<ApplicationIdentityUser, int>(new ApplicationUserStore<ApplicationIdentityUser, int>(database, 1));
            ApplicationIdentityUser user = manager.FindByEmail(message.Destination);

            var data = new EmailConfirmationModel();
            data.EmailAddress = message.Destination;
            data.PortalName = user.Portals[0].Name;
            data.Name = string.Format("{0} {1}", user.FirstName, user.LastName);
            data.Parameters = message.Body;

            switch (message.Subject)
            {
                case "EmailConfirm":
                    //send message to user for validation
                    messageService = new MessageService(user.Portals.FirstOrDefault().Id, null);
                    await messageService.SendEmailAsync("EMLVLD", message.Destination, data);

                    break;
                case "PasswordReset":

                    //send message to user for validation
                    messageService = new MessageService(user.Portals.FirstOrDefault().Id, null);
                    await messageService.SendEmailAsync("PASRST", message.Destination, data);

                    break;
                case "PasswordResetConfirm":

                    //send message to user to inform about password change
                    messageService = new MessageService(user.Portals.FirstOrDefault().Id, null);
                    await messageService.SendEmailAsync("PASCFR", message.Destination, data);

                    break;
            }


        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            //add later
            return;
        }


    }
}