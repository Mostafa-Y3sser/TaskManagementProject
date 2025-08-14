using FluentValidation;

namespace Task_Management.Application.UserOperations.RevokeToken
{
    public class RevokeRefreshTokenValidator : AbstractValidator<RevokeRefreshTokenCommand>
    {
        public RevokeRefreshTokenValidator()
        {
            RuleFor(x => x.Token)
                    .NotEmpty().WithMessage("Refresh Token is required.");
        }
    }
}