using Dot.Application.Interfaces;
using Dot.Core.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _config;
        public MailService(IOptions<MailSettings> mailSettings, IConfiguration config)
        {
            _mailSettings = mailSettings.Value;
            _config = config;
        }
        public async Task<bool> SendRequestEmailAsync(MailRequest mailRequest)
        {
            try
            {
                string filePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", mailRequest.ToEmail).Replace("[email]", mailRequest.ToEmail);
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Body;
                var builder = new BodyBuilder();
                builder.HtmlBody = mailText;
                email.Body = builder.ToMessageBody();
                /* if(mailRequest.Attachments != null)
                 {
                     byte[] fileBytes;
                     foreach (var file in mailRequest.Attachments)
                     {
                         if (file.Length > 0)
                         {
                             using (var mss = new MemoryStream())
                             {
                                 *//*file.CopyTo(mss);*//*
                                 fileBytes = mss.ToArray();
                             }
                             builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                         }
                     }
                 }*/
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using (var smtp = new SmtpClient())
                {
                    smtp.CheckCertificateRevocation = false;
                    await smtp.ConnectAsync(_config["MailSettings:Host"], 465, true); //MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> SendWelcomeEmailAsync(WelcomeRequest mailRequest)
        {
            try
            {
                string filePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", mailRequest.UserName).Replace("[email]", mailRequest.ToEmail);
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = $"Welcome to Dot, {mailRequest.UserName}";
                var builder = new BodyBuilder();
                builder.HtmlBody = mailText;
                email.Body = builder.ToMessageBody();
                using(var smtp = new SmtpClient())
                {
                    smtp.CheckCertificateRevocation = false;
                    await smtp.ConnectAsync(_config["MailSettings:Host"], 465, true); //MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
