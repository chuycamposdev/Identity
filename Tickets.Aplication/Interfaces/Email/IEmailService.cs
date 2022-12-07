using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Dtos.Email;
using Tickets.Aplication.Models;

namespace Tickets.Aplication.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendPlainTextEmailAsync(EmailModel request);
        Task SendHTMLEmailAsync(EmailModel request);
    }
}
