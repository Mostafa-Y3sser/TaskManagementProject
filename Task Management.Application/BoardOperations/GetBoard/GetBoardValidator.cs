using FluentValidation;

namespace Task_Management.Application.BoardOperations.GetBoard
{
    public class GetBoardValidator : AbstractValidator<GetBoardQuery>
    {
        public GetBoardValidator()
        {
            RuleFor(query => query.BoardID)
                .NotEmpty().WithMessage("Project ID is required.")
                .GreaterThan(0).WithMessage("Project ID must be a positive integer.");
        }
    }
}