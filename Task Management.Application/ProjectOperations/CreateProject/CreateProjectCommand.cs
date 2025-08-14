using MediatR;

namespace Task_Management.Application.ProjectOperations.CreateProject
{
    public class CreateProjectCommand : IRequest<int>
    {
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
