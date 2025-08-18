using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.HomeOperations.GetAllProjects
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
    }
}
