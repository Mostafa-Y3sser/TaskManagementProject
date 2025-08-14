using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.UserOperations.refreshToken
{
    public class RefreshTokenCommand : IRequest<AuthGeneralResponse>
    {
        public string Token { get; set; } = string.Empty;
    }
}
