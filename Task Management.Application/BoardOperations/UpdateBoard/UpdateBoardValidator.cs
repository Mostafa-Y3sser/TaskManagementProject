using FluentValidation;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.BoardOperations.UpdateBoard
{
    public class UpdateBoardValidator : AbstractValidator<UpdateBoardCommand>
    {

        public UpdateBoardValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            RuleFor(x => x.BoardName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Board name is required.");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Description can't exceed 100 characters.");

            RuleFor(x => x.ProjectID)
                .GreaterThan(0).When(x => x.ProjectID.HasValue)
                .WithMessage("Project ID must be a positive integer.");
        }
    }
}