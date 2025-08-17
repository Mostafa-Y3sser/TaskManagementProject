using MediatR;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Application.Dtos;
using Task_Management.Application.ProjectOperations.CreateProject;
using Task_Management.Application.ProjectOperations.GetProject;
using Task_Management.Domain.Interfaces;
using Task_Management.Responses;
using Task_Management.Application.ProjectOperations.UpdateProject;
using Task_Management.Application.ProjectOperations.DeleteProject;

namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateProject")]
        public async Task<ActionResult<ApiResponse<int>>> CreateProject([FromBody] CreateProjectCommand command)
        {
            int ProjectID = await _mediator.Send(command);

            return Ok(ApiResponse<int>.Success(ProjectID));
        }

        [HttpGet("GetProject/{ID:int}")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject([FromRoute] int ID)
        {
            ProjectDto projectDto = await _mediator.Send(new GetProjectQuery { ProjectID = ID });

            return Ok(ApiResponse<ProjectDto>.Success(projectDto));
        }

        [HttpPut("UpdateProject/{ID:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProject([FromRoute] int ID, [FromBody] UpdateProjectCommand command)
        {
            command.ProjectID = ID;

            bool Result = await _mediator.Send(command);

            return Ok(ApiResponse<bool>.Success(Result, "Project updated successfully."));
        }

        [HttpDelete("DeleteProject/{ID:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProject([FromRoute] int ID)
        {
            bool Result = await _mediator.Send(new DeleteProjectCommand { ProjectID = ID });

            return Ok(ApiResponse<bool>.Success(Result, "Project deleted successfully."));
        }
    }
}