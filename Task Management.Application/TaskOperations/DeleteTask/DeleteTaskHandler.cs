using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.TaskOperations.DeleteTask
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTaskHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated.");

            TaskItem? task = await _unitOfWork.TaskRepository.GetAsync(c => c.ID == command.TaskID && c.Board!.UserID == UserID);
            if (task == null)
                throw new NotFoundException("Task not found or does not belong to the user.");

            await _unitOfWork.TaskRepository.RemoveAsync(task.ID);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}