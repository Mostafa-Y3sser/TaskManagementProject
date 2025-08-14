using MediatR;

namespace Task_Management.Application.ProjectOperations.DeleteProject
{
    public class DeleteProjectCommand : IRequest<bool>
    {
        public int ProjectID { get; set; }
    }
}
