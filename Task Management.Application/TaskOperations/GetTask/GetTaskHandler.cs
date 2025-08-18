using MediatR;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.TaskOperations.GetTask
{
    public class GetTaskHandler : IRequestHandler<GetTaskQuery, TaskDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetTaskHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<TaskDto> Handle(GetTaskQuery query, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated.");

            TaskItem? task = await _unitOfWork.TaskRepository.GetAsync(c => c.ID == query.TaskID && c.Board!.UserID == UserID);
            if (task == null)
                throw new NotFoundException($"Task with ID {query.TaskID} not found.");

            TaskDto taskDto = new TaskDto
            {
                TaskTitle = task.TaskTitle,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
            };

            return taskDto;
        }
    }
}