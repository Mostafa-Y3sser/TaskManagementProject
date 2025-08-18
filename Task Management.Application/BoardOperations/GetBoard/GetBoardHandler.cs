using MediatR;
using Task_Management.Application.Dtos;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.BoardOperations.GetBoard
{
    public class GetBoardHandler : IRequestHandler<GetBoardQuery, BoardDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public GetBoardHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<BoardDto> Handle(GetBoardQuery query, CancellationToken cancellationToken)
        {
            String? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Board? board = await _unitOfWork.BoardRepository.GetAsync(b => b.UserID == UserID && b.ID == query.BoardID);
            if (board == null)
                throw new NotFoundException($"Board with ID {query.BoardID} not found.");

            BoardDto boardDto = new()
            {
                BoardName = board.BoardName,
                Description = board.Description ?? string.Empty,
                CreatedAt = board.CreatedAt,
            };

            return boardDto;
        }
    }
}