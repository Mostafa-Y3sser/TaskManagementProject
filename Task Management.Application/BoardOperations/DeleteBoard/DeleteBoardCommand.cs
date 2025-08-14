using MediatR;

namespace Task_Management.Application.BoardOperations.DeleteBoard
{
    public class DeleteBoardCommand : IRequest<bool>
    {
        public int BoardID { get; set; }
    }
}