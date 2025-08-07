using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.User.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, AuthGeneralResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public CreateUserHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthGeneralResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser User = new()
            {
                FullName = $"{command.FirstName} {command.LastName}",
                Email = command.Email,
                UserName = new MailAddress(command.Email).User
            };

            IdentityResult Result = await _userManager.CreateAsync(User, command.Password);
            if (!Result.Succeeded)
                throw new CreateUserException($"User creation failed due to : " +
                    $"{String.Join(", ", Result.Errors.Select(e => e.Description))}");

            JwtSecurityToken accessToken = await _jwtTokenService.CreateAccessTokenAsync(User);
            RefreshToken refreshToken = await _jwtTokenService.CreateRefreshTokenAsync(User.Id);
            _jwtTokenService.SetRefreshTokenInCookie(refreshToken);

            return new AuthGeneralResponse()
            {
                Data = "User created successfully.",
                FullName = User.FullName,
                Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                ExpirationOn = accessToken.ValidTo
            };
        }
    }
}
