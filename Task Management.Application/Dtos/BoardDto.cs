using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Management.Application.Dtos
{
    public class BoardDto
    {
        public int BoardID { get; set; }
        public string BoardName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<TaskDto> TaskDtos { get; set; } = new();
    }
}