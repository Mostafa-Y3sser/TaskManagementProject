using System;

namespace Task_Management.Domain.Entities
{
    public class TaskActivity
    {
        public int ID { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;

        public int TaskItemID { get; set; }
        public TaskItem Task { get; set; } = new TaskItem();

        public string? UserID { get; set; }
        public ApplicationUser User { get; set; } = new ApplicationUser();
    }
}
