using Task_Management.Domain.Enums;

namespace Task_Management.Domain.Entities
{
    public class TaskItem
    {
        public int ID { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public enTaskStatus Status { get; set; } = enTaskStatus.ToDo;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }

        public int BoardID { get; set; }
        public Board? Board { get; set; }

        public List<TaskActivity>? TaskActivities { get; set; }

    }
}
