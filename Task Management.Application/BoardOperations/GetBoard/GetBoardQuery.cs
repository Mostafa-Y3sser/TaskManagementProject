using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.BoardOperations.GetBoard
{
    public class GetBoardQuery : IRequest<BoardDto>
    {
        public int BoardID { get; set; }
    }
}