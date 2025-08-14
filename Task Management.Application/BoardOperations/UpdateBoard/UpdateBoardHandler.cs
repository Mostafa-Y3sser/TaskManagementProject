using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.BoardOperations.UpdateBoard
{
    class UpdateBoardHandler : IRequestHandler<UpdateBoardCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBoardHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateBoardCommand command, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Board? board = await _unitOfWork.BoardRepository.GetAsync(b => b.ID == command.BoardID && b.UserID == UserID);
            if (board == null)
                throw new NotFoundException($"Board with ID {command.BoardID} not found.");

            if (command.ProjectID.HasValue)
            {
                Project? project = await _unitOfWork.ProjectRepository.GetAsync(p => p.ID == command.ProjectID && p.UserID == UserID);
                if (project == null)
                    throw new NotFoundException($"Project with ID {command.ProjectID} not found.");

                board.ProjectID = command.ProjectID.Value;
            }
            else if (board.ProjectID.HasValue && command.ProjectID == null)
            {
                board.ProjectID = null;
            }

            board.BoardName = command.BoardName;
            board.Description = command.Description ?? string.Empty;

            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}