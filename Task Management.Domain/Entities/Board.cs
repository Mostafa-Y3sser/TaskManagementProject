using System;

namespace Task_Management.Domain.Entities
{
    public class Board
    {
        public int ID { get; set; }
        public string BoardName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? UserID { get; set; }
        public ApplicationUser? User { get; set; }

        public int? ProjectID { get; set; }
        public Project? Project { get; set; }

        public List<TaskItem>? Tasks { get; set; }
    }
}
