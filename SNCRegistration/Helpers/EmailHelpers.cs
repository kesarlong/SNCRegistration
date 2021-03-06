﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace SNCRegistration.Helpers
{
    public static class EmailHelpers
    {        
        public static void SendEmail(string fAddress, string tAddress, string subj, string bdy, string pdfFolder)
        {
            var pass = System.Configuration.ConfigurationManager.AppSettings["emailPassword"];
            var fromAddress = new MailAddress(fAddress);
            var toAddress = new MailAddress(tAddress);
            var bcc = "sncracc@gmail.com";
            string fromPassword = pass;

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
                message.Attachments.Add(new Attachment(pdfFolder + "MediaRelease.pdf"));
                message.Attachments.Add(new Attachment(pdfFolder + "ParticipantHealthForm.pdf"));
                message.Attachments.Add(new Attachment(pdfFolder + "ParticipantWelcomePacket.pdf"));
                smtp.Send(message);
            }
        }

        public static void SendVolEmail(string fAddress, string tAddress, string subj, string bdy, string pdfFolder)
        {
            var pass = System.Configuration.ConfigurationManager.AppSettings["emailPassword"];
            var fromAddress = new MailAddress(fAddress);
            var toAddress = new MailAddress(tAddress);
            var bcc = "sncracc@gmail.com";
            string fromPassword = pass;

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
                IsBodyHtml = true,

            })
            {
                message.Bcc.Add(new MailAddress(bcc));

                message.Attachments.Add(new Attachment(pdfFolder + "VolunteerInformation.pdf"));
                message.Attachments.Add(new Attachment(pdfFolder + "VolunteerHealthForm.pdf"));
                smtp.Send(message);
            }
        }
    }
}