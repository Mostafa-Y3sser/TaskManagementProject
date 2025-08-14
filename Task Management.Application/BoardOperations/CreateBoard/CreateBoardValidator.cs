using FluentValidation;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.BoardOperations.CreateBoard
{
    public class CreateBoardValidator : AbstractValidator<CreateBoardCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateBoardValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;

            RuleFor(x => x.BoardName)
           .Cascade(CascadeMode.Stop)
           .NotEmpty().WithMessage("Board name is required.")
           .MaximumLength(30).WithMessage("Board name can't exceed 30 characters.")
           .MustAsync((Model, BoardName, CancellationToken) => BeUniqueName(Model, BoardName, CancellationToken))
              .WithMessage("This board name already exists.");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Description can't exceed 100 characters.");

            RuleFor(x => x.ProjectID)
                .GreaterThan(0).When(x => x.ProjectID.HasValue)
                .WithMessage("Project ID must be greater than zero if provided.");
        }

        private async Task<bool> BeUniqueName(CreateBoardCommand command, string BoardName, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                return false;

            if (command.ProjectID.HasValue)
            {
                Project? project = await _unitOfWork.ProjectRepository.GetAsync(p => p.ID == command.ProjectID.Value && p.UserID == UserID);
                if (project == null)
                    return false;

                return !await _unitOfWork.BoardRepository.AnyAsync(b => b.BoardName.ToLower() == BoardName.ToLower() && b.ProjectID == project.ID, cancellationToken);
            }

            return !await _unitOfWork.BoardRepository.AnyAsync(b => b.BoardName.ToLower() == BoardName.ToLower() && b.UserID == UserID
            && !b.ProjectID.HasValue, cancellationToken);
        }
    }
}