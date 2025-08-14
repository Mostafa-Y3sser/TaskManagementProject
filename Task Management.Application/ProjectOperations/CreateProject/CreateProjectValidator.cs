using FluentValidation;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.ProjectOperations.CreateProject
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateProjectValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;

            RuleFor(x => x.ProjectName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(50).WithMessage("Project name must not exceed 50 characters.")
                .MustAsync((ProjectName, CancellationToken) => BeUniqueName(ProjectName, CancellationToken))
                .WithMessage("This name is already exist.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(50).WithMessage("Description must not exceed 50 characters.");
        }

        private async Task<bool> BeUniqueName(string ProjectName, CancellationToken cancellationToken)
        {
            String UserID = _currentUserService.UserID;

            return !await _unitOfWork.ProjectRepository.AnyAsync(c => c.ProjectName.ToLower() == ProjectName.ToLower()
            && c.UserID == UserID, cancellationToken);
        }
    }
}