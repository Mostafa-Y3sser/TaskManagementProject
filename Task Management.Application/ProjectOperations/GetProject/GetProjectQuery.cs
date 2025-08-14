using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.ProjectOperations.GetProject
{
    public class GetProjectQuery : IRequest<ProjectDto>
    {
        public int ProjectID { get; set; }
    }
}