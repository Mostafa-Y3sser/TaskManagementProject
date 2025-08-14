using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.ProjectOperations.CreateProject
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateProjectHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            String UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Project project = new()
            {
                ProjectName = command.ProjectName,
                Description = command.Description,
                UserID = UserID,
            };

            await _unitOfWork.ProjectRepository.AddAsync(project);
            await _unitOfWork.SaveAsync();

            return project.ID;
        }
    }
}