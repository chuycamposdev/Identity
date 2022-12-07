using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Dtos.Email;
using Tickets.Aplication.Interfaces.Email;
using Tickets.Aplication.Models;
using Tickets.Domain.Settings;

namespace Tickets.Infraestructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSetting;

        public EmailService(IOptions<EmailSetting> mailSettings)
        {
            _emailSetting = mailSettings.Value;
        }

        public async Task SendPlainTextEmailAsync(EmailModel request)
        {
            var email = InitializeEmail(request);
            email.Body = new TextPart(TextFormat.Plain) { Text = request.Message };
            await SendEmailAsync(email);
        }

        public async Task SendHTMLEmailAsync(EmailModel request)
        {
            var email = InitializeEmail(request);
            email.Body = new TextPart(TextFormat.Html) { Text = request.Message };
            await SendEmailAsync(email);
        }

        private MimeMessage InitializeEmail(EmailModel emailRequest)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSetting.Mail));
            email.To.Add(MailboxAddress.Parse(emailRequest.To));
            email.Subject = emailRequest.Subject;
            return email;
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSetting.Mail, _emailSetting.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }


    }
}
