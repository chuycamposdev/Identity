using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Features.Account.Commands;
using Tickets.Aplication.Features.Account.Login;
using Tickets.Aplication.Interfaces.Account;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(SigninUserCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("register-account")]
        public async Task<IActionResult> RegisterUser(RegisterUserCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = await _accountService.RefreshToken(refreshTokenDto);
            return Ok(refreshToken);
        }
    }
}
