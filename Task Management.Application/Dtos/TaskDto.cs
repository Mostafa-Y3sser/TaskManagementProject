using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Management.Domain.Enums;

namespace Task_Management.Application.Dtos
{
    public class TaskDto
    {
        public int TaskID { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public enTaskStatus Status { get; set; } = enTaskStatus.ToDo;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
    }
}
