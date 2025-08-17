using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.TaskOperations.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateTaskHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated.");

            Board? board = await _unitOfWork.BoardRepository.GetAsync(c => c.ID == command.BoardID && c.UserID == UserID);
            if (board == null)
                throw new NotFoundException($"Board with ID {command.BoardID} not found.");

            TaskItem task = new()
            {
                TaskTitle = command.TaskTitle,
                DueDate = command.DueDate,
                BoardID = command.BoardID
            };

            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.SaveAsync();

            return task.ID;
        }
    }
}