using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Aplication.Dtos.Authorization
{
    public record LoginDto(string Email, string Password);
}
