using MediatR;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.ProjectOperations.UpdateProject
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProjectHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            String UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Project? project = await _unitOfWork.ProjectRepository.GetAsync(p => p.ID == command.ProjectID && p.UserID == UserID);
            if (project is null)
                throw new NotFoundException($"Project with ID {command.ProjectID} not found.");

            project.ProjectName = command.ProjectName;
            project.Description = command.Description;

            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}