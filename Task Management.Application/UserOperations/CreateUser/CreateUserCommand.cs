using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.UserOperations.CreateUser
{
    public class CreateUserCommand : IRequest<AuthGeneralResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
