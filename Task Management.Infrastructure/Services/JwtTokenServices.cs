using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Infrastructure.Persistence.Data;

namespace Task_Management.Infrastructure.Services
{
    class JwtTokenServices : IJwtTokenService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenServices(IConfiguration configuration, UserManager<ApplicationUser> userManager, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<JwtSecurityToken> CreateAccessTokenAsync(ApplicationUser User)
        {
            List<Claim> UserClaims = new List<Claim>();
            UserClaims.AddRange(
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, User.Id),
                new Claim(ClaimTypes.Name, User.FullName),
                new Claim(ClaimTypes.Email, User.Email!)
                );

            List<string> UserRoles = (await _userManager.GetRolesAsync(User)).ToList();
            foreach (string Role in UserRoles)
                UserClaims.Add(new Claim(ClaimTypes.Role, Role));

            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            SigningCredentials SigningCred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken accessToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: UserClaims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT: TokenExpirationInMinutes"])),
                signingCredentials: SigningCred
                );

            return accessToken;
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(string UserID)
        {
            byte[] RandomBytes = new byte[64];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(RandomBytes);

            RefreshToken refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomBytes),
                CreatedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(7),
                UserID = UserID
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public void SetRefreshTokenInCookie(RefreshToken refreshToken)
        {
            if (refreshToken is not null && refreshToken.IsActive)
            {
                CookieOptions cookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = refreshToken.ExpiresOn,
                    SameSite = SameSiteMode.Strict
                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken.Token, cookieOptions);
            }
        }

        public async Task<bool> RevokeTokenAsync(string RefreshToken)
        {
            if (String.IsNullOrWhiteSpace(RefreshToken))
                throw new TokenException("Refresh token is required");

            RefreshToken? StoredRefreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(RF => RF.Token == RefreshToken && (RF.RevokedOn == null && RF.ExpiresOn >= DateTime.UtcNow));

            if (StoredRefreshToken is null)
                return false;

            StoredRefreshToken.RevokedOn = DateTime.UtcNow;
            _context.RefreshTokens.Update(StoredRefreshToken);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AuthGeneralResponse> RefreshTokenAsync(string RefreshToken)
        {
            if (string.IsNullOrEmpty(RefreshToken))
                throw new TokenException("Refresh token is required");

            RefreshToken? StoredRefreshToken = await _context.RefreshTokens
               .SingleOrDefaultAsync(RF => RF.Token == RefreshToken && (RF.RevokedOn == null && RF.ExpiresOn >= DateTime.UtcNow));

            if (StoredRefreshToken is null)
                throw new TokenException("Invalid or expired Refresh Token");

            ApplicationUser? User = await _userManager.FindByIdAsync(StoredRefreshToken.UserID);
            if (User is null)
                throw new TokenException("There isn't any user assigned to this Refresh Token.");

            StoredRefreshToken.RevokedOn = DateTime.UtcNow;
            _context.RefreshTokens.Update(StoredRefreshToken);
            await _context.SaveChangesAsync();

            JwtSecurityToken jwtToken = await CreateAccessTokenAsync(User);
            RefreshToken refreshToken = await CreateRefreshTokenAsync(User.Id);
            SetRefreshTokenInCookie(refreshToken);

            return new AuthGeneralResponse
            {
                FullName = User.FullName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                ExpirationOn = jwtToken.ValidTo,
                Data = "The new token generated successfully. "
            };
        }
    }
}