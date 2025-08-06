using FluentValidation;

namespace Task_Management.Application.User.RevokeToken
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