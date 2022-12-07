using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Aplication.Models
{
    public record EmailModel(string To, string Subject, string Message);

}
