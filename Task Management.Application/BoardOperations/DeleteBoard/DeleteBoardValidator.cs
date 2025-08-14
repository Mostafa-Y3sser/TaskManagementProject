using FluentValidation;

namespace Task_Management.Application.BoardOperations.DeleteBoard
{
    public class DeleteBoardValidator : AbstractValidator<DeleteBoardCommand>
    {
        public DeleteBoardValidator()
        {
            RuleFor(command => command.BoardID)
                .NotEmpty().WithMessage("Board ID is required")
                .GreaterThan(0).WithMessage("Board ID must be a positive integer");
        }
    }
}