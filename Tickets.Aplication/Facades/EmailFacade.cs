using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Interfaces.Email;
using Tickets.Aplication.Models;

namespace Tickets.Aplication.Facades
{
    public class EmailFacade
    {
        private readonly IEmailService _emailService;

        public EmailFacade(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendEmailRegistration(string email, string username, string refreskToken)
        {
            
            string message = $"Please {username}, confirm your account in order to log in";
            string subject = "Email account confirmation";
            await _emailService.SendHTMLEmailAsync(new EmailModel(email, subject, message));
        }
    }
}
