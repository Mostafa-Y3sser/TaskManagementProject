using FluentValidation;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.ProjectOperations.UpdateProject
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProjectValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;

            RuleFor(x => x.ProjectID)
                .NotEmpty().WithMessage("Project ID is required.");

            RuleFor(x => x.ProjectName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage("Project name is required.")
               .MaximumLength(50).WithMessage("Project name must not exceed 50 characters.")
               .MustAsync((Model, ProjectName, CancellationToken) => BeUniqueName(Model.ProjectID, ProjectName, CancellationToken))
               .WithMessage("This name is already exist.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(50).WithMessage("Description must not exceed 50 characters.");
        }

        private async Task<bool> BeUniqueName(int ProjectID, string ProjectName, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.ProjectRepository.AnyAsync(c => c.ProjectName.ToLower() == ProjectName.ToLower() && c.ID != ProjectID
            && c.UserID == _currentUserService.UserID, cancellationToken);
        }
    }
}