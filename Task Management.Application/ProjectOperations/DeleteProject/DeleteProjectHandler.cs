using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.ProjectOperations.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteProjectHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken cancellationToken = default)
        {
            String? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Project? project = await _unitOfWork.ProjectRepository.GetAsync(p => p.ID == command.ProjectID && p.UserID == UserID);
            if (project == null)
                throw new NotFoundException($"Project with ID {command.ProjectID} not found.");

            await _unitOfWork.ProjectRepository.RemoveAsync(project.ID);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}