using Task_Management.Domain.Enums;

namespace Task_Management.Application.Dtos
{
    public class TaskDto
    {
        public string TaskTitle { get; set; } = string.Empty;
        public enTaskStatus Status { get; set; } = enTaskStatus.ToDo;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
    }
}