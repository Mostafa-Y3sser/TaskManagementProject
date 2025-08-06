using MediatR;
using Microsoft.AspNetCore.Http;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;

namespace Task_Management.Application.User.refreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthGeneralResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenHandler(IHttpContextAccessor httpContextAccessor, IJwtTokenService jwtTokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthGeneralResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            AuthGeneralResponse Response = await _jwtTokenService.RefreshTokenAsync(command.Token);
            return Response;
        }
    }
}
