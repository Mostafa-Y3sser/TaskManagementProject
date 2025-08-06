using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
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

        public async Task<AuthGeneralResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser User = new()
            {
                FullName = $"{request.FirstName} {request.LastName}",
                Email = request.Email,
                UserName = new MailAddress(request.Email).User
            };

            JwtSecurityToken accessToken = await _jwtTokenService.CreateAccessTokenAsync(User);
            RefreshToken refreshToken = await _jwtTokenService.CreateRefreshTokenAsync(User.Id);
            _jwtTokenService.SetRefreshTokenInCookie(refreshToken);

            IdentityResult Result = await _userManager.CreateAsync(User, request.Password);
            if (!Result.Succeeded)
                throw new ApplicationException($"User creation failed due to : " +
                    $"{String.Join(", ", Result.Errors.Select(e => e.Description))}");

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
