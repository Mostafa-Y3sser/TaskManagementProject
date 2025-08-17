using MediatR;

namespace Task_Management.Application.TaskOperations.CreateTask
{
    public class CreateTaskCommand : IRequest<int>
    {
        public string TaskTitle { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }

        public int BoardID { get; set; }
    }
}