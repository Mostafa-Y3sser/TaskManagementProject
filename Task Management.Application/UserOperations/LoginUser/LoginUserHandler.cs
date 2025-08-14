using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;

namespace Task_Management.Application.UserOperations.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthGeneralResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginUserHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthGeneralResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser? User = await _userManager.FindByNameAsync(command.UserNameOrEmail) ??
                await _userManager.FindByEmailAsync(command.UserNameOrEmail);

            if (User is null || !await _userManager.CheckPasswordAsync(User, command.Password))
                throw new LoginUserException("Invalid User Name or Password.");

            JwtSecurityToken accessToken = await _jwtTokenService.CreateAccessTokenAsync(User);
            RefreshToken refreshToken = await _jwtTokenService.CreateRefreshTokenAsync(User.Id);
            _jwtTokenService.SetRefreshTokenInCookie(refreshToken);

            return new AuthGeneralResponse
            {
                Data = "User logged in successfully.",
                FullName = User.FullName,
                Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                ExpirationOn = accessToken.ValidTo,
            };
        }
    }
}
