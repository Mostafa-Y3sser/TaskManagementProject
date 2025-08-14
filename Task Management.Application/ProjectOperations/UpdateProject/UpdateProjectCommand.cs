using System.Text.Json.Serialization;
using MediatR;

namespace Task_Management.Application.ProjectOperations.UpdateProject
{
    public class UpdateProjectCommand : IRequest<bool>
    {
        [JsonIgnore]
        public int ProjectID { get; set; }

        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
