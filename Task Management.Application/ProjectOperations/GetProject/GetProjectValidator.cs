using FluentValidation;

namespace Task_Management.Application.ProjectOperations.GetProject
{
    public class GetProjectValidator : AbstractValidator<GetProjectQuery>
    {
        public GetProjectValidator()
        {
            RuleFor(query => query.ProjectID)
                .NotEmpty().WithMessage("Project ID is required.")
                .GreaterThan(0).WithMessage("Project ID must be greater than zero.");
        }
    }
}