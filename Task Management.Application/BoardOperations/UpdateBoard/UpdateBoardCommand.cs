using System.Text.Json.Serialization;
using MediatR;

namespace Task_Management.Application.BoardOperations.UpdateBoard
{
    public class UpdateBoardCommand : IRequest<bool>
    {
        [JsonIgnore]
        public int BoardID { get; set; }

        public string BoardName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? ProjectID { get; set; }
    }
}