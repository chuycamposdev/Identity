using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Models;

namespace Tickets.Aplication.Features.Account.Login
{
    public class SigninUserCommand : IRequest<ResponseModel<UserDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
