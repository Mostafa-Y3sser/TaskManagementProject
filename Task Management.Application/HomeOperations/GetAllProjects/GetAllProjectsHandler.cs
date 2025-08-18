using MediatR;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.HomeOperations.GetAllProjects
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetAllProjectsHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }


        public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated.");

            IEnumerable<Project> projects = await _unitOfWork.ProjectRepository.GetAllAsync(b => b.UserID == UserID);
            if (projects == null || !projects.Any())
                throw new NotFoundException("No projects found for the user.");

            return projects.Select(b => new ProjectDto
            {
                ProjectID = b.ID,
                ProjectName = b.ProjectName,
                Description = b.Description ?? string.Empty,
            });
        }
    }
}