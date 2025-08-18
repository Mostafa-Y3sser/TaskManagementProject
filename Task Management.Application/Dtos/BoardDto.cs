using System;

namespace Task_Management.Application.Dtos
{
    public class BoardDto
    {
        public string BoardName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}