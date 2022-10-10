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
        Task<ResponseModel<string>> RegisterAccountAsync(RegisterDto request);
        Task<ResponseModel<UserDto>> LoginAsync(LoginDto requestModel);
        Task<ResponseModel<RefreshTokenDto>> RefreshToken(RefreshTokenDto refreshTokenDto);
    }
}
