using MediatR;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.HomeOperations.GetAllBoards
{
    public class GetAllBoardsHandler : IRequestHandler<GetAllBoardsQuery, IEnumerable<BoardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetAllBoardsHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<BoardDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
        {
            string? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated.");

            IEnumerable<Board> boards = await _unitOfWork.BoardRepository.GetAllAsync(b => b.UserID == UserID && !b.ProjectID.HasValue);
            if (boards == null || !boards.Any())
                throw new NotFoundException("No independent boards found for the user.");

            return boards.Select(b => new BoardDto
            {
                BoardName = b.BoardName,
                Description = b.Description ?? string.Empty,
                CreatedAt = b.CreatedAt
            });
        }
    }
}