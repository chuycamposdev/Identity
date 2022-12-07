using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Web;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Exceptions;
using Tickets.Aplication.Interfaces.Account;
using Tickets.Aplication.Models;
using Tickets.Domain.Settings;
using Tickets.Infraestructure.Identity.Models;
using Tickets.Infraestructure.Shared;

namespace Tickets.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private const string basicRole = "BASIC";

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<string> RegisterAccountAsync(RegisterModel request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                throw new ApiException("User already exists");
            ApplicationUser user = _mapper.Map<ApplicationUser>(request);
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                string confirmationToken = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
                await _userManager.AddToRoleAsync(user, basicRole);
            }
            else
            {
                ThrowRegisterErrorMessage(result);
            }

            return user.Id;
        }


        public async Task<string> GenerateConfirmationTokenAsync(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            return HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
        }

        public async Task<UserDto> LoginAsync(LoginModel loginModel)
        {
            var user = await ValidateUserCredentials(loginModel);
            var userRoles = await _userManager.GetRolesAsync(user);
            string token = await _jwtTokenGenerator.GenerateJWTTokenAsync(loginModel.Email);
            string refreshToken = await _jwtTokenGenerator.GenerateRefreshToken(user.Id, token);
            return new UserDto(
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.FirstName,
                user.LastName,
                token,
                userRoles.ToList(),
                refreshToken);
        }

        public async Task<RefreshTokenDto> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = await _jwtTokenGenerator.ValidateRefreshToken(refreshTokenDto);
            return new RefreshTokenDto(refreshToken.RefreshToken, refreshToken.Token, refreshToken.UserId);
        }

        private async Task<ApplicationUser> ValidateUserCredentials(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                throw new InvalidCredentialsException();
            var isUserValid = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!isUserValid.Succeeded)
                throw new InvalidCredentialsException();
            return user;
        }

        private void ThrowRegisterErrorMessage(IdentityResult result)
        {
            string errorMessage = "Something went wrong";
            if (result.Errors.Any())
                errorMessage = result.Errors.FirstOrDefault().Description;
            throw new ApiException(errorMessage);
        }
    }
}
