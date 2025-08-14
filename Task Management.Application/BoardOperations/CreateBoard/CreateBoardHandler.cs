using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.BoardOperations.CreateBoard
{
    public class CreateBoardHandler : IRequestHandler<CreateBoardCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateBoardHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateBoardCommand command, CancellationToken cancellationToken)
        {
            String? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Board board = new()
            {
                BoardName = command.BoardName,
                Description = command.Description ?? string.Empty
            };

            if (command.ProjectID.HasValue)
            {

                Project? project = await _unitOfWork.ProjectRepository.GetAsync(c => c.ID == command.ProjectID && c.UserID == UserID);
                if (project == null)
                    throw new NotFoundException($"Project with ID {command.ProjectID} not found.");

                board.ProjectID = command.ProjectID.Value;
            }

            board.UserID = UserID;

            await _unitOfWork.BoardRepository.AddAsync(board);
            await _unitOfWork.SaveAsync();

            return board.ID;
        }
    }
}