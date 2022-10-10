using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Web;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Aplication.Exceptions;
using Tickets.Aplication.Interfaces.Account;
using Tickets.Aplication.Models;
using Tickets.Domain.Settings;
using Tickets.Infraestructure.Identity.Models;

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

        public async Task<ResponseModel<string>> RegisterAccountAsync(RegisterDto request)
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

            return new ResponseModel<string>(user.Id, "Congratulations, user registered succesfully");
        }

        public async Task<ResponseModel<UserDto>> LoginAsync(LoginDto requestModel)
        {
            var user = await ValidateUserCredentials(requestModel);
            var userRoles = await _userManager.GetRolesAsync(user);
            string token = await _jwtTokenGenerator.GenerateJWTTokenAsync(requestModel.Email);
            string refreshToken = await _jwtTokenGenerator.GenerateRefreshToken(user.Id, token);
            return new ResponseModel<UserDto>(new UserDto(
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.FirstName,
                user.LastName,
                token,
                userRoles.ToList(),
                refreshToken), string.Empty
            );

        }

        public async Task<ResponseModel<RefreshTokenDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = await _jwtTokenGenerator.ValidateRefreshToken(refreshTokenDto);
            return new ResponseModel<RefreshTokenDto>(refreshToken, string.Empty);
        }

        private async Task<ApplicationUser> ValidateUserCredentials(LoginDto loginModel)
        {
            var jwt = new JwtSettings();
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
