using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Aplication.Dtos.Authorization
{
    public record UserDto(string Id, string Email, string PhoneNumber, string FirstName,
        string LastName, string Token, List<string> roles, string RefreshToken);
}
