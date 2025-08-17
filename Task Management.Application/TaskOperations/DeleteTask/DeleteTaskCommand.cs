using MediatR;

namespace Task_Management.Application.TaskOperations.DeleteTask
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public int TaskID { get; set; }
    }
}