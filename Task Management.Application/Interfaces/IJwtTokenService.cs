using System.IdentityModel.Tokens.Jwt;
using Task_Management.Application.Dtos;
using Task_Management.Domain.Entities;

namespace Task_Management.Application.Interfaces
{
    public interface IJwtTokenService
    {
        Task<JwtSecurityToken> CreateAccessTokenAsync(ApplicationUser User);
        Task<RefreshToken> CreateRefreshTokenAsync(string UserID);
        void SetRefreshTokenInCookie(RefreshToken refreshToken);
        Task<bool> RevokeTokenAsync(string RefreshToken);
        Task<AuthGeneralResponse> RefreshTokenAsync(string refreshToken);
    }
}
