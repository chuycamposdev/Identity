using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Facades;
using Tickets.Aplication.Interfaces.Account;
using Tickets.Aplication.Interfaces.Email;
using Tickets.Aplication.Models;

namespace Tickets.Aplication.Features.Account.Commands
{
    public class RegisteUserHandler : IRequestHandler<RegisterUserCommand, ResponseModel<string>>
    {
        private readonly IAccountService _accountService;
        private readonly EmailFacade _emailFacade;

        public RegisteUserHandler(IAccountService accountService, EmailFacade emailFacade)
        {
            _accountService = accountService;
            _emailFacade = emailFacade;
        }

        public async Task<ResponseModel<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var registerModel = new RegisterModel(request.FirstName, request.LastName, request.Email,
                                                    request.PhoneNumber, request.Password);

            string userId = await _accountService.RegisterAccountAsync(registerModel);
            string confirmationToken = await _accountService.GenerateConfirmationTokenAsync(userId);
            await _emailFacade.SendEmailRegistration(registerModel.Email, registerModel.FirstName, confirmationToken);

            return new ResponseModel<string>("User added succesfully");
        }
    }
}
