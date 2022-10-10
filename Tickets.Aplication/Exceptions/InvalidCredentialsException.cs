using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Aplication.Exceptions
{
    public class InvalidCredentialsException: ApiException
    {
        private const string errorMessage = "Email/Password Invalid";
        public InvalidCredentialsException() : base(errorMessage)
        {

        }
    }
}
