using MediatR;
using System.Text.Json.Serialization;
using Task_Management.Domain.Enums;

namespace Task_Management.Application.TaskOperations.UpdateTask
{
    public class UpdateTaskCommand : IRequest<bool>
    {
        [JsonIgnore]
        public int TaskID { get; set; }

        public string TaskTitle { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public enTaskStatus Status { get; set; }
    }
}