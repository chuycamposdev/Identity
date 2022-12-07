using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Models;

namespace Tickets.Aplication.Interfaces.Account
{
    public interface IAccountService
    {
        Task<string> RegisterAccountAsync(RegisterModel request);
        Task<UserDto> LoginAsync(LoginModel requestModel);
        Task<RefreshTokenDto> RefreshToken(RefreshTokenDto refreshTokenDto);
        Task<string> GenerateConfirmationTokenAsync(string userID);
    }
}
