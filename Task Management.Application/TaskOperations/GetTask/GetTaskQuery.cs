using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.TaskOperations.GetTask
{
    public class GetTaskQuery : IRequest<TaskDto>
    {
        public int TaskID { get; set; }
    }
}