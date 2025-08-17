using FluentValidation;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.TaskOperations.CreateTask
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateTaskValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;

            RuleFor(x => x.TaskTitle)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(100).WithMessage("Task title cannot exceed 100 characters.")
                .MustAsync((Model, TaskTitle, cancellationToken) => BeUniqueTitle(Model, TaskTitle, cancellationToken))
                .WithMessage("A task with this title already exists in the board.");

            RuleFor(x => x.DueDate)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(DateTime.Now).WithMessage("Due date must be in the future.")
                .LessThanOrEqualTo(DateTime.Now.AddDays(2)).WithMessage("Due date cannot be more than two days in the future.");

            RuleFor(x => x.BoardID)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Board ID is required.")
                .GreaterThan(0).WithMessage("Board ID must be a valid positive integer.");
        }

        private async Task<bool> BeUniqueTitle(CreateTaskCommand command, string TaskTitle, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                return false;

            Board? board = await _unitOfWork.BoardRepository.GetAsync(c => c.ID == command.BoardID && c.UserID == UserID);
            if (board == null)
                return false;

            return !await _unitOfWork.TaskRepository.AnyAsync(c => c.TaskTitle.ToLower() == TaskTitle && c.BoardID == board.ID, cancellationToken);
        }
    }
}