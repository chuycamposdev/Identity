using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Dtos.Authorization;

namespace Tickets.Aplication.Interfaces.Account
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateJWTTokenAsync(string email);
        Task<string> GenerateRefreshToken(string userId, string token);
        Task<RefreshTokenDto> ValidateRefreshToken(RefreshTokenDto refreshTokenDto);
    }
}
