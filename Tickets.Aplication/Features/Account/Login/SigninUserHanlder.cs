using MediatR;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Interfaces.Account;
using Tickets.Aplication.Models;

namespace Tickets.Aplication.Features.Account.Login
{
    public class SigninUserHanlder : IRequestHandler<SigninUserCommand, ResponseModel<UserDto>>
    {
        private readonly IAccountService _accountService;

        public SigninUserHanlder(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ResponseModel<UserDto>> Handle(SigninUserCommand request, CancellationToken cancellationToken)
        {
            var login = new LoginModel(request.Email, request.Password);
            var user = await _accountService.LoginAsync(login);
            return new ResponseModel<UserDto>(user);
        }
    }
}
