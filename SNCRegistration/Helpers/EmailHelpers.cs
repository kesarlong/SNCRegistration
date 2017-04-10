using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace SNCRegistration.Helpers
{
    public static class EmailHelpers
    {        
        public static void SendEmail(string fAddress, string tAddress, string subj, string bdy)
        {
            var fromAddress = new MailAddress(fAddress);
            var toAddress = new MailAddress(tAddress);
            var bcc = "sncracc@gmail.com";
            const string fromPassword = "Coffee1$";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subj,
                Body = bdy,

            })
            {
                message.Bcc.Add(new MailAddress(bcc));
                message.Attachments.Add(new Attachment("c:\\temp\\MediaRelease.pdf"));
                message.Attachments.Add(new Attachment("c:\\temp\\ParticipantHealthForm.pdf"));
                message.Attachments.Add(new Attachment("c:\\temp\\ParticipantWelcomePacket.pdf"));
                smtp.Send(message);
            }
        }

        public static void SendVolEmail(string fAddress, string tAddress, string subj, string bdy)
        {
            var fromAddress = new MailAddress(fAddress);
            var toAddress = new MailAddress(tAddress);
            var bcc = "sncracc@gmail.com";
            const string fromPassword = "Coffee1$";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subj,
                Body = bdy,

            })
            {
                message.Bcc.Add(new MailAddress(bcc));
                message.Attachments.Add(new Attachment("c:\\temp\\EventInfo.pdf"));
                message.Attachments.Add(new Attachment("c:\\temp\\VolunteerHealthForm.pdf"));
                smtp.Send(message);
            }
        }
    }
}