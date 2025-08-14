using MediatR;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Exceptions;
using Task_Management.Domain.Interfaces;

namespace Task_Management.Application.BoardOperations.DeleteBoard
{
    public class DeleteBoardHandler : IRequestHandler<DeleteBoardCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteBoardHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteBoardCommand command, CancellationToken cancellationToken)
        {
            String? UserID = _currentUserService.UserID;
            if (string.IsNullOrWhiteSpace(UserID))
                throw new UnauthorizedAccessException("User is not authenticated");

            Board? board = await _unitOfWork.BoardRepository.GetAsync(b => b.ID == command.BoardID && b.UserID == UserID);
            if (board == null)
                throw new NotFoundException($"Board with ID {command.BoardID} not found.");

            await _unitOfWork.BoardRepository.RemoveAsync(board.ID);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}