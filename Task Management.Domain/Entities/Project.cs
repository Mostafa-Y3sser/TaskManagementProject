using System;

namespace Task_Management.Domain.Entities
{
    public class Project
    {
        public int ID { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid UserID { get; set; }
        public ApplicationUser? User { get; set; }

        public List<Board>? Boards { get; set; }
    }
}
