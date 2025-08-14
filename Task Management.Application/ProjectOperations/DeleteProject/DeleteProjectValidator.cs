using FluentValidation;

namespace Task_Management.Application.ProjectOperations.DeleteProject
{
    public class DeleteProjectValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectValidator()
        {
            RuleFor(command => command.ProjectID)
                .NotEmpty().WithMessage("Project ID is required.")
                .GreaterThan(0).WithMessage("Project ID must be a positive integer.");
        }
    }
}