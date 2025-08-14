using MediatR;

namespace Task_Management.Application.BoardOperations.CreateBoard
{
    public class CreateBoardCommand : IRequest<int>
    {
        public string BoardName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? ProjectID { get; set; }
    }
}