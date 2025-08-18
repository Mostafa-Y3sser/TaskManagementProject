using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.HomeOperations.GetAllBoards
{
    public class GetAllBoardsQuery : IRequest<IEnumerable<BoardDto>>
    {
    }
}