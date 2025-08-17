using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.TaskOperations.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTaskHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated.");

            TaskItem? task = await _unitOfWork.TaskRepository.GetAsync(c => c.ID == command.TaskID && c.Board!.UserID == UserID);
            if (task == null)
                throw new NotFoundException($"Task with ID {command.TaskID} not found.");

            task.TaskTitle = command.TaskTitle;
            if (command.DueDate.HasValue)
                task.DueDate = command.DueDate.Value;
            task.Status = command.Status;

            _unitOfWork.TaskRepository.Update(task);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}