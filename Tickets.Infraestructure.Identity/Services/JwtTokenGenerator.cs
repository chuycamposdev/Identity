using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Exceptions;
using Tickets.Aplication.Interfaces.Account;
using Tickets.Domain.Entities;
using Tickets.Domain.Settings;
using Tickets.Infraestructure.Identity.Models;

namespace Tickets.Infraestructure.Identity.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TicketDBContext _ticketDBContext;

        public JwtTokenGenerator(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> options,
            TicketDBContext dBContext
        )
        {
            _userManager = userManager;
            _jwtSettings = options.Value;
            _ticketDBContext = dBContext;
        }

        public async Task<string> GenerateJWTTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ApiException("User not found, token was not generated");
            var roles = await _userManager.GetRolesAsync(user);

            //Identify user by adding claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<RefreshTokenDto> ValidateRefreshToken(RefreshTokenDto refreshToken)
        {
            await RefreshTokenValidations(refreshToken);
            var user = await _userManager.FindByIdAsync(refreshToken.UserId);
            var token = await GenerateJWTTokenAsync(user.Email);
            var newRefreshToken = await GenerateRefreshToken(user.Id, token);
            

            return new RefreshTokenDto(newRefreshToken, token, user.Id);
        }

        public async Task<string> GenerateRefreshToken(string userId, string token)
        {
            var refreshToken = new RefreshToken
            {
                Active = true,
                Created = DateTime.Now,
                Expiration = DateTime.Now.AddDays(1),
                RefreshTokenValue = Guid.NewGuid().ToString(),
                Used = false,
                UserId = userId,
                Token = token
            };

            await DisableLastRefreshTokenActive(userId);
            await _ticketDBContext.AddAsync(refreshToken);
            await _ticketDBContext.SaveChangesAsync();

            return refreshToken.RefreshTokenValue;
        }

        private async Task DisableLastRefreshTokenActive(string userId)
        {
            var lastTokenActive = await _ticketDBContext.RefeshToken.FirstOrDefaultAsync(x => x.UserId == userId && x.Active);
            if (lastTokenActive != null)
            {
                lastTokenActive.Active = false;
                lastTokenActive.Used = true;
                _ticketDBContext.Update(lastTokenActive);
                await _ticketDBContext.SaveChangesAsync();
            }
        }

        private async Task RefreshTokenValidations(RefreshTokenDto refreshToken)
        {
            var activeToken = await _ticketDBContext.RefeshToken.FirstOrDefaultAsync(
                                x => x.UserId == refreshToken.UserId
                                && x.RefreshTokenValue == refreshToken.RefreshToken);

            if (activeToken == null)
            {
                throw new ApiException("Active token was not found");
            }

            if (!activeToken.Active)
            {
                throw new ApiException("Active Token Not Actived");
            }

            if (activeToken.Expiration <= DateTime.Now)
            {
                throw new ApiException("Refresh token expired");
            }
        }
    }
}
