using MediatR;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.ProjectOperations.GetProject
{
    public class GetProjectHandler : IRequestHandler<GetProjectQuery, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetProjectHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ProjectDto> Handle(GetProjectQuery query, CancellationToken cancellationToken = default)
        {
            String UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Project? project = await _unitOfWork.ProjectRepository.GetAsync(Filter: p => p.ID == query.ProjectID && p.UserID == UserID,
            IncludeProperties: "Boards");
            if (project == null)
                throw new NotFoundException($"Project with ID {query.ProjectID} not found.");

            ProjectDto projectDto = new ProjectDto
            {
                ProjectName = project.ProjectName,
                Description = project.Description,
            };

            return projectDto;
        }
    }
}