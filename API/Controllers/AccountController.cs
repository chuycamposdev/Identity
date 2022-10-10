using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Interfaces.Account;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //[Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var userDto = await _accountService.LoginAsync(loginDto);
            return Ok(userDto);
        }

        [HttpPost("register-account")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto register)
        {
            var account = await _accountService.RegisterAccountAsync(register);
            return Ok(account);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = await _accountService.RefreshToken(refreshTokenDto);
            return Ok(refreshToken);
        }
    }
}
