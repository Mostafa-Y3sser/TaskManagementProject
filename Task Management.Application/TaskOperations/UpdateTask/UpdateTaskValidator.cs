using FluentValidation;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.TaskOperations.UpdateTask
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTaskValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;

            RuleFor(x => x.TaskID)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Task ID is required.")
                .GreaterThan(0).WithMessage("Task ID must be greater than 0.");

            RuleFor(x => x.TaskTitle)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(100).WithMessage("Task title must not exceed 100 characters.")
                .MustAsync((model, TaskTitle, CancellationToken) => BeUniqueTitle(model, TaskTitle, CancellationToken))
                .WithMessage("A task with this title already exists in the board.");

            RuleFor(x => x.DueDate)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(DateTime.Now).When(x => x.DueDate.HasValue)
                .WithMessage("Due date must be today or in the future.")
                .LessThanOrEqualTo(DateTime.Now.AddDays(2)).When(x => x.DueDate.HasValue)
                .WithMessage("Due date cannot be more than two days in the future.");

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .IsInEnum().WithMessage("Status must be a valid task status.")
                .NotEmpty().WithMessage("Status is required.");
        }

        private async Task<bool> BeUniqueTitle(UpdateTaskCommand command, string TaskTitle, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                return false;

            TaskItem? task = await _unitOfWork.TaskRepository.GetAsync(c => c.ID == command.TaskID && c.Board!.UserID == UserID, IncludeProperties: "Board");
            if (task == null)
                return false;


            Board? board = await _unitOfWork.BoardRepository.GetAsync(c => c.ID == task.BoardID);
            if (board == null)
                return false;

            return !await _unitOfWork.TaskRepository.AnyAsync(c => c.TaskTitle.ToLower() == TaskTitle && c.ID != command.TaskID && c.BoardID == board.ID, cancellationToken);
        }
    }
}