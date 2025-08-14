using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Task_Management.Application.UserOperations.LoginUser
{
    public class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username or Email is required.")
                .Length(2, 30).WithMessage("Username or Email must be between 2 and 30 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
